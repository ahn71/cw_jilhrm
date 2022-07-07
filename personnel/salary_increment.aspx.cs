using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System.Data;
using SigmaERP.classes;

namespace SigmaERP.personnel
{
    public partial class salary_increment : System.Web.UI.Page
    {
        DataTable dt;
        string query = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();                
                loadSalaryInfo();
                classes.commonTask.LoadGrade(ddlGrade);
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                ViewState["__CompanyId__"].ToString();
                Office_IsGarments();
              
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "salary_increment.aspx", ddlCompany, divSalaryIncrementList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                if (ViewState["__ReadAction__"].ToString().Equals("0")) 
                {
                    btnIncrementInfo.Enabled = false;
                    btnIncrementInfo.CssClass = "";
                }

                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();

            

                classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, ViewState["__CompanyId__"].ToString());
                LoadAllownceSetting();

            }
            catch { }

        }

        private void Office_IsGarments()
        {
            try
            {

                if (classes.Payroll.Office_IsGarments())
                {                    
                    hfIsGarments.Value = "1";
                }
                else
                {                  
                    hfIsGarments.Value = "0";
                }
            }
            catch { }
            
        }

        private void findLastSalaryIncrement()
        {
            try
            {
                dt = new DataTable();
                dt = Payroll.SalaryInfo(ddlEmpCardNo.SelectedValue);               
                if (dt.Rows.Count > 0)
                {                   
                    txtEmpName.Text = dt.Rows[0]["EmpName"].ToString();
                    txtLastIncrementDate.Text =Convert.ToDateTime(dt.Rows[0]["DateofUpdate"].ToString()).ToString("dd-MM-yyyy");
                    txtLastIncrementAmount.Text = dt.Rows[0]["IncrementAmount"].ToString();
                    ViewState["__getPresentSalary__"] = dt.Rows[0]["EmpPresentSalary"].ToString();                 
                    hdfSalaryType.Value = dt.Rows[0]["SalaryType"].ToString();                    
                    SetConstraint(dt.Rows[0]["EmpTypeId"].ToString());
                    
                    string grade = dt.Rows[0]["GrdName"].ToString();
                    if (grade != "")
                    {
                        txtPresentGrade.Text = dt.Rows[0]["GrdName"].ToString();
                        ddlGrade.Text = dt.Rows[0]["GrdName"].ToString();
                    }                   
                    txtPresentGrossSalary.Text = dt.Rows[0]["EmpPresentSalary"].ToString();
                    txtPresentBasic.Text= dt.Rows[0]["BasicSalary"].ToString();
                    txtPresentMedical.Text = dt.Rows[0]["MedicalAllownce"].ToString();
                    lblHouseRent.Text = dt.Rows[0]["HouseRent_Persent"].ToString();
                    hfTotalHouseRent.Value = dt.Rows[0]["HouseRent_Persent"].ToString();

                    txtPresentHouseRent.Text = dt.Rows[0]["HouseRent"].ToString();
                    txtPresentConveyance.Text = dt.Rows[0]["ConvenceAllownce"].ToString();
                    txtPresentFoodAllowance.Text = dt.Rows[0]["FoodAllownce"].ToString();
                    
                    lblPF.Text = dt.Rows[0]["PF_Persent"].ToString();
                    txtOthers.Text = dt.Rows[0]["OthersAllownce"].ToString();                    
                    txtNewTDS.Text= txtTDS.Text = dt.Rows[0]["IncomeTax"].ToString();                    
                   
                    if (dt.Rows[0]["PfMember"].ToString() == "True")
                    {
                        
                        trPFAmount.Visible = true;
                    }
                    else
                    {
                      
                        trPFAmount.Visible = false;
                    }                  
                    txtPFAmount.Text = dt.Rows[0]["PFAmount"].ToString();
                    hdfSalaryType.Value = dt.Rows[0]["SalaryType"].ToString();
                    
                    SetConstraint(dt.Rows[0]["EmpTypeId"].ToString());
                   
                    
                }
            }
            catch { }
        }

        private void SetConstraint(string EmpTypeId)
        {
            try
            {



                // here 0=Percentage 1=Amount 2=Not Count

                // 0=SalaryType, 1=Basic, 2=MedicalAllownce, 3=FoodAllownce, 4=ConvenceAllownce, 5=TechnicalAllowance, 6=HouseRent, 7=OthersAllowance, 8=PFAllowance

                DataRow[] dr;
                dr = dt_AllowanceSettings.Select("EmpTypeId=" + EmpTypeId + "");
                if (dr.Length >= 1)
                {

                    ViewState["__AlCalId__"] = dr[0]["AlCalId"].ToString();



                    if (dr[0]["BasicStatus"].ToString() == "0" || dr[0]["BasicStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblBasic.Text = " ( " + dr[0]["BasicAllowance"].ToString() + " % )";
                        lblBasic.ForeColor = System.Drawing.Color.Blue;
                        hdfBasic.Value = dr[0]["BasicAllowance"].ToString();
                    }
                    else if (dr[0]["BasicStatus"].ToString() == "1") // 1 =৳
                    {
                        lblBasic.Text = " ( ৳ )";
                        lblBasic.ForeColor = System.Drawing.Color.Green;
                        if (hfIsGarments.Value == "1")
                        {
                            if (txtPresentBasic.Text.Trim() == "0" || txtPresentBasic.Text.Trim() == " ")
                            {
                                txtPresentBasic.Text = dr[0]["BasicAllowance"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblBasic.Text = " ( x )";
                        lblBasic.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Basic Allowance Part---------------------------------------

                    if (dr[0]["MedicalStatus"].ToString() == "0" || dr[0]["MedicalStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblMedical.Text = " ( " + dr[0]["MedicalAllownce"].ToString() + " % )";
                        lblMedical.ForeColor = System.Drawing.Color.Blue;
                        hdfMedical.Value = dr[0]["MedicalAllownce"].ToString();
                    }
                    else if (dr[0]["MedicalStatus"].ToString() == "1") // 1 =৳
                    {
                        lblMedical.Text = " ( ৳ )";
                        lblMedical.ForeColor = System.Drawing.Color.Green;

                        if (hfIsGarments.Value == "1")
                        {
                            if (txtPresentMedical.Text.Trim() == "0" || txtPresentMedical.Text.Trim() == " ")
                            {
                                txtPresentMedical.Text = dr[0]["MedicalAllownce"].ToString();
                            }
                        }
                        else
                        {
                            if (txtPresentMedical.Text.Trim() == "0" || txtPresentMedical.Text.Trim() == " ")
                            {
                                txtPresentMedical.Text = dr[0]["MedicalAllownce"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblMedical.Text = " ( x )";
                        lblMedical.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Medical Allowance Part---------------------------------------

                    if (dr[0]["FoodStatus"].ToString() == "0" || dr[0]["FoodStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblFood.Text = " ( " + dr[0]["FoodAllownce"].ToString() + " % )";
                        lblFood.ForeColor = System.Drawing.Color.Blue;
                        hdfFoodAllowance.Value = dr[0]["FoodAllownce"].ToString();
                    }
                    else if (dr[0]["FoodStatus"].ToString() == "1") // 1 =৳
                    {
                        lblFood.Text = " ( ৳ )";
                        lblFood.ForeColor = System.Drawing.Color.Green;
                        if (hfIsGarments.Value == "1")
                        {
                            if (txtPresentFoodAllowance.Text.Trim() == "0" || txtPresentFoodAllowance.Text.Trim() == " ")
                            {
                                txtPresentFoodAllowance.Text = dr[0]["FoodAllownce"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblFood.Text = " ( x )";
                        lblFood.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Food Allowance Part---------------------------------------

                    if (dr[0]["ConStatus"].ToString() == "0" || dr[0]["ConStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblConveyance.Text = " ( " + dr[0]["ConvenceAllownce"].ToString() + " % )";
                        lblConveyance.ForeColor = System.Drawing.Color.Blue;
                        hdfConveyance.Value = dr[0]["ConvenceAllownce"].ToString();
                    }
                    else if (dr[0]["ConStatus"].ToString() == "1") // 1 =৳
                    {
                        lblConveyance.Text = " ( ৳ )";
                        lblConveyance.ForeColor = System.Drawing.Color.Green;
                        if (hfIsGarments.Value == "1")
                        {
                            if (txtPresentConveyance.Text.Trim() == "0" || txtPresentConveyance.Text.Trim() == " ")
                            {
                                txtPresentConveyance.Text = dr[0]["ConvenceAllownce"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblConveyance.Text = " ( x )";
                        lblConveyance.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Convence Allowance Part---------------------------------------

                    if (dr[0]["TecStatus"].ToString() == "0" || dr[0]["TecStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblTechnical.Text = " ( " + dr[0]["TechnicalAllowance"].ToString() + " % )";
                        lblTechnical.ForeColor = System.Drawing.Color.Blue;
                        hdfTechnical.Value = dr[0]["TechnicalAllowance"].ToString();
                    }
                    else if (dr[0]["TecStatus"].ToString() == "1") // 1 =৳
                    {
                        lblTechnical.Text = " ( ৳ )";
                        lblTechnical.ForeColor = System.Drawing.Color.Green;
                        if (hfIsGarments.Value == "1")
                        {
                            if (txtTechnicalAllow.Text.Trim() == "0" || txtTechnicalAllow.Text.Trim() == " ")
                            {
                                txtTechnicalAllow.Text = dr[0]["TechnicalAllowance"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblTechnical.Text = " ( x )";
                        lblTechnical.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Technical Allowance Part---------------------------------------

                    if (dr[0]["HouseStatus"].ToString() == "0" || dr[0]["HouseStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblHouseRent.Text = " ( " + dr[0]["HouseRent"].ToString() + " % )";
                        lblHouseRent.ForeColor = System.Drawing.Color.Blue;
                        hdfhouserent.Value = dr[0]["HouseRent"].ToString();
                    }
                    else if (dr[0]["HouseStatus"].ToString() == "1") // 1 =৳
                    {
                        lblHouseRent.Text = " ( ৳ )";
                        lblHouseRent.ForeColor = System.Drawing.Color.Green;

                        if (hfIsGarments.Value == "1")
                        {
                            if (txtPresentHouseRent.Text.Trim() == "0" || txtPresentHouseRent.Text.Trim() == " ")
                            {
                                txtPresentHouseRent.Text = dr[0]["HouseRent"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblHouseRent.Text = " ( x )";
                        lblHouseRent.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End House Rent Allowance Part---------------------------------------

                    if (dr[0]["OthStatus"].ToString() == "0" || dr[0]["OthStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblOthers.Text = " ( " + dr[0]["OthersAllowance"].ToString() + " % )";
                        lblOthers.ForeColor = System.Drawing.Color.Blue;
                        hdfOthers.Value = dr[0]["OthersAllowance"].ToString();
                    }
                    else if (dr[0]["OthStatus"].ToString() == "1") // 1 =৳
                    {
                        lblOthers.Text = " ( ৳ )";
                        lblOthers.ForeColor = System.Drawing.Color.Green;

                        if (hfIsGarments.Value == "1")
                        {
                            if (txtOthers.Text.Trim() == "0" || txtOthers.Text.Trim() == " ")
                            {
                                txtOthers.Text = dr[0]["OthersAllowance"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblOthers.Text = " ( x )";
                        lblOthers.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Others Rent Allowance Part---------------------------------------


                    if (dr[0]["PFStatus"].ToString() == "0" || dr[0]["PFStatus"].ToString() == "3") // 0 or 3 =% 
                    {
                        lblPF.Text = " ( " + dr[0]["PFAllowance"].ToString() + " % )";
                        lblPF.ForeColor = System.Drawing.Color.Blue;
                        hdfPF.Value = dr[0]["PFAllowance"].ToString();
                    }
                    else if (dr[0]["PFStatus"].ToString() == "1") // 1 =৳
                    {
                        lblPF.Text = " ( ৳ )";
                        lblPF.ForeColor = System.Drawing.Color.Green;

                        if (hfIsGarments.Value == "1")
                        {
                            if (txtPFAmount.Text.Trim() == "0" || txtPFAmount.Text.Trim() == " ")
                            {
                                txtPFAmount.Text = dr[0]["PFAllowance"].ToString();
                            }
                        }
                    }

                    else  // 2 =x
                    {
                        lblPF.Text = " ( x )";
                        lblPF.ForeColor = System.Drawing.Color.Red;
                    }
                    //--------------------------------End Provident Fund Allowance Part---------------------------------------


                    hfBasicStatus.Value = dr[0]["BasicStatus"].ToString();
                    hfMedicalStatus.Value = dr[0]["MedicalStatus"].ToString();
                    hfFoodStatus.Value = dr[0]["FoodStatus"].ToString();
                    hfConveyanceStatus.Value = dr[0]["ConStatus"].ToString();
                    hfTechnicalStatus.Value = dr[0]["TecStatus"].ToString();
                    hfHouseStatus.Value = dr[0]["HouseStatus"].ToString();
                    hfOthersStatus.Value = dr[0]["OthStatus"].ToString();
                    hfPFStatus.Value = dr[0]["PFStatus"].ToString();
                }
                else
                {
                    lblMessage.InnerText = "error->Please set the salary constrant before salary set.";
                }
            }
            catch { }
        }

        static DataTable dt_AllowanceSettings = new DataTable();
        private void LoadAllownceSetting()
        {
            try
            {
                dt_AllowanceSettings = new DataTable();
                sqlDB.fillDataTable("Select acs.SalaryType ,has.BasicAllowance,has.MedicalAllownce,has.FoodAllownce,has.ConvenceAllownce,has.TechnicalAllowance, " +
                    " has.HouseRent,has.OthersAllowance,has.PFAllowance,has.AlCalId,acs.EmpTypeId," +
                    "acs.BasicAllowance as BasicStatus,acs.MedicalAllownce as MedicalStatus,acs.FoodAllownce as FoodStatus,acs.ConvenceAllownce as ConStatus," +
                    " acs.TechnicalAllowance as TecStatus,acs.HouseRent as HouseStatus,acs.OthersAllowance as OthStatus, acs.ProvidentFund as PFStatus " +
                    " from HRD_AllownceSetting as has inner join Payroll_AllowanceCalculationSetting acs on has.AlCalId=acs.AlCalId where CalculationType='salary' ", dt_AllowanceSettings);

            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {                                                        
           findLastSalaryIncrement();                             
        }

        private bool IsBankSalary()
        {
            try
            {
                DataTable dtSalaryCount=new DataTable ();
                sqlDB.fillDataTable(" select SalaryCount from v_EmployeeDetails where SN='" + ddlEmpCardNo.SelectedValue.ToString() + "' and IsActive='1'", dtSalaryCount);
                if (dtSalaryCount.Rows[0]["SalaryCount"].ToString() == "Bank") return true;
                else return false;
            }
            catch { return false; }
        
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {           
            if (hfSaveStatus.Value == "Save")
            {
                if(ddlEmpCardNo.SelectedIndex<1)
                {
                    lblMessage.InnerText = "warning-> Please select any employee !"; ddlEmpCardNo.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                if (txtIncrementAmount.Text.Trim().Length < 1) 
                {
                    lblMessage.InnerText = "warning-> Please type increament amount !"; txtIncrementAmount.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true); 
                    return;
                }
                if (ddlGrade.SelectedIndex < 1)
                {
                    lblMessage.InnerText = "warning-> Please select salary grade!"; ddlGrade.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                if (txtIncrementOrderRefDate.Text.Trim().Length < 7)
                {
                    lblMessage.InnerText = "warning-> Please select order ref date !"; txtIncrementOrderRefDate.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                if (txtEffectiveFrom.Text.Trim().Length < 7)
                {
                    lblMessage.InnerText = "warning-> Please select effective month !"; txtEffectiveFrom.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                saveSalaryIncrement();
            }
               
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                loadSalaryInfo();
                
            }
        }

        private void saveSalaryIncrement()
        {
            try
            {
                DataTable dtPreStatus = new DataTable();
                sqlDB.fillDataTable("Select TiffinAllownce,NightAllownce,AttendanceBonus,EarnLeaveDate,LunchAllownce,LunchCount,PreDptId,PreDsgId,PreGrdName,CompanyId,EmpId,EmpCardNo,PreEmpTypeId,EmpTypeId,PreSalaryType,SalaryType,EmpPresentSalary,IncrementAmount,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,PreTechnicalAllownce,TechnicalAllownce,DptId,DsgId,EmpStatus,GrdName,OthersAllownce,EarnLeaveDate,PreShiftTransferDate,ShiftTransferDate,ShiftTransferToDate,SftId,GId,PreGId,SalaryCount,BankId,EmpAccountNo,PfMember,PfDate,PFAmount,CustomOrdering,OverTime,PreEmpDutyType,EmpDutyType,EmpJoinigSalary,DormitoryRent,PreIncomeTax,IncomeTax From Personnel_EmpCurrentStatus where  SN='" + ddlEmpCardNo.SelectedValue + "' ", dtPreStatus);
                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpCurrentStatus (EmpId, PreCompanyId, CompanyId, EmpCardNo, PreEmpTypeId, EmpTypeId, PreSalaryType, SalaryType, PreEmpSalary, EmpPresentSalary, PreIncrementAmount, IncrementAmount, PreBasicSalary, BasicSalary,PreMedicalAllownce, MedicalAllownce,PreFoodAllownce, FoodAllownce,PreConvenceAllownce, ConvenceAllownce, PreHouseRent, HouseRent,PreTechnicalAllownce,TechnicalAllownce, PreDptId, DptId, PreDsgId, DsgId, PreEmpStatus, EmpStatus, PreGrdName, GrdName, PreOthersAllownce, OthersAllownce, HolidayAllownce, TiffinAllownce, NightAllownce, AttendanceBonus, LunchAllownce, LunchCount, DateofUpdate, TypeOfChange, EffectiveMonth, OrderRefNo, OrderRefDate, Remarks, ActiveSalary, EarnLeaveDate, IsActive,PreShiftTransferDate,ShiftTransferDate,ShiftTransferToDate,SftId,GId,PreGId,SalaryCount,BankId,EmpAccountNo,PfMember,PfDate,PFAmount,CustomOrdering,OverTime,PreEmpDutyType,EmpDutyType,EmpJoinigSalary,DormitoryRent,PreIncomeTax,IncomeTax)  values (@EmpId, @PreCompanyId, @CompanyId, @EmpCardNo, @PreEmpTypeId, @EmpTypeId, @PreSalaryType, @SalaryType, @PreEmpSalary, @EmpPresentSalary, @PreIncrementAmount, @IncrementAmount, @PreBasicSalary, @BasicSalary,@PreMedicalAllownce, @MedicalAllownce,@PreFoodAllownce,@FoodAllownce,@PreConvenceAllownce, @ConvenceAllownce, @PreHouseRent, @HouseRent,@PreTechnicalAllownce,@TechnicalAllownce, @PreDptId, @DptId, @PreDsgId, @DsgId, @PreEmpStatus, @EmpStatus, @PreGrdName, @GrdName, @PreOthersAllownce, @OthersAllownce, @HolidayAllownce, @TiffinAllownce, @NightAllownce, @AttendanceBonus, @LunchAllownce, @LunchCount, @DateofUpdate, @TypeOfChange, @EffectiveMonth, @OrderRefNo, @OrderRefDate, @Remarks, @ActiveSalary, @EarnLeaveDate, @IsActive,@PreShiftTransferDate,@ShiftTransferDate,@ShiftTransferToDate,@SftId,@GId,@PreGId,@SalaryCount,@BankId,@EmpAccountNo,@PfMember,@PfDate,@PFAmount,@CustomOrdering,@OverTime,@PreEmpDutyType,@EmpDutyType,@EmpJoinigSalary,@DormitoryRent,@PreIncomeTax,@IncomeTax) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", dtPreStatus.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@PreCompanyId", dtPreStatus.Rows[0]["CompanyId"].ToString());
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                    cmd.Parameters.AddWithValue("@CompanyId", ddlCompany.SelectedValue);
                else cmd.Parameters.AddWithValue("@CompanyId", ViewState["__CompanyId__"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", dtPreStatus.Rows[0]["EmpCardNo"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpTypeId", dtPreStatus.Rows[0]["PreEmpTypeId"].ToString());
                cmd.Parameters.AddWithValue("@EmpTypeId", dtPreStatus.Rows[0]["EmpTypeId"].ToString());
                cmd.Parameters.AddWithValue("@PreSalaryType", dtPreStatus.Rows[0]["PreSalaryType"].ToString());
                cmd.Parameters.AddWithValue("@SalaryType", dtPreStatus.Rows[0]["SalaryType"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpSalary", dtPreStatus.Rows[0]["EmpPresentSalary"].ToString());
                cmd.Parameters.AddWithValue("@EmpPresentSalary", txtNewGross.Text.Trim());
                cmd.Parameters.AddWithValue("@PreIncrementAmount", dtPreStatus.Rows[0]["IncrementAmount"].ToString());
                cmd.Parameters.AddWithValue("@IncrementAmount", txtIncrementAmount.Text.Trim());
                cmd.Parameters.AddWithValue("@PreBasicSalary", dtPreStatus.Rows[0]["BasicSalary"].ToString());
                cmd.Parameters.AddWithValue("@BasicSalary", txtNewBasic.Text.Trim());
                cmd.Parameters.AddWithValue("@PreMedicalAllownce", dtPreStatus.Rows[0]["MedicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce", txtNewMedical.Text.Trim());
                cmd.Parameters.AddWithValue("@PreFoodAllownce", dtPreStatus.Rows[0]["FoodAllownce"].ToString());
                cmd.Parameters.AddWithValue("@FoodAllownce", txtNewFoodAllowance.Text.Trim());
                cmd.Parameters.AddWithValue("@PreConvenceAllownce", dtPreStatus.Rows[0]["ConvenceAllownce"].ToString());
                cmd.Parameters.AddWithValue("@ConvenceAllownce", txtNewConveyance.Text.Trim());
                cmd.Parameters.AddWithValue("@PreHouseRent", dtPreStatus.Rows[0]["HouseRent"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", txtNewHouseRent.Text.Trim());
                cmd.Parameters.AddWithValue("@PreTechnicalAllownce", dtPreStatus.Rows[0]["PreTechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@TechnicalAllownce", dtPreStatus.Rows[0]["TechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@PreDptId", dtPreStatus.Rows[0]["PreDptId"].ToString());
                cmd.Parameters.AddWithValue("@DptId", dtPreStatus.Rows[0]["DptId"].ToString());
                cmd.Parameters.AddWithValue("@PreDsgId", dtPreStatus.Rows[0]["PreDsgId"].ToString());
                cmd.Parameters.AddWithValue("@DsgId", dtPreStatus.Rows[0]["DsgId"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpStatus", dtPreStatus.Rows[0]["EmpStatus"].ToString());
                cmd.Parameters.AddWithValue("@EmpStatus", "1");
                cmd.Parameters.AddWithValue("@PreGrdName", dtPreStatus.Rows[0]["PreGrdName"].ToString());
                cmd.Parameters.AddWithValue("@GrdName", ddlGrade.Text.Trim());
                cmd.Parameters.AddWithValue("@PreOthersAllownce", dtPreStatus.Rows[0]["OthersAllownce"].ToString());
                cmd.Parameters.AddWithValue("@OthersAllownce",txtNewTransportOthers.Text.Trim());
                cmd.Parameters.AddWithValue("@HolidayAllownce", "0");
                cmd.Parameters.AddWithValue("@TiffinAllownce", dtPreStatus.Rows[0]["TiffinAllownce"].ToString());
                cmd.Parameters.AddWithValue("@NightAllownce", dtPreStatus.Rows[0]["NightAllownce"].ToString());
                if (hfIsGarments.Value == "1" && int.Parse(txtNewGross.Text)<=5500)
                {
                    cmd.Parameters.AddWithValue("@AttendanceBonus", "200");
                }
                else cmd.Parameters.AddWithValue("@AttendanceBonus", "0");
                cmd.Parameters.AddWithValue("@LunchAllownce", dtPreStatus.Rows[0]["LunchAllownce"].ToString());
                byte getLounchCount = (bool.Parse(dtPreStatus.Rows[0]["LunchCount"].ToString()) == true) ? (byte)1 : (byte)0;
                cmd.Parameters.AddWithValue("@LunchCount", getLounchCount.ToString());
                cmd.Parameters.AddWithValue("@DateofUpdate", convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")));
                cmd.Parameters.AddWithValue("@TypeOfChange", "i");
                cmd.Parameters.AddWithValue("@EffectiveMonth", txtEffectiveFrom.Text);
                cmd.Parameters.AddWithValue("@OrderRefNo", txtIncrementOrderRefNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@OrderRefDate", convertDateTime.getCertainCulture(txtIncrementOrderRefDate.Text));
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@ActiveSalary", 0);
                if (dtPreStatus.Rows[0]["EarnLeaveDate"].ToString() == "" || dtPreStatus.Rows[0]["EarnLeaveDate"].ToString() == " ") cmd.Parameters.AddWithValue("@EarnLeaveDate", convertDateTime.getCertainCulture("01-01-2050").ToString());
                else cmd.Parameters.AddWithValue("@EarnLeaveDate", dtPreStatus.Rows[0]["EarnLeaveDate"].ToString());
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
               // cmd.Parameters.AddWithValue("@PFAmount", dtPreStatus.Rows[0]["PFAmount"].ToString());
                cmd.Parameters.AddWithValue("@PFAmount", txtNewPF.Text.Trim());
                cmd.Parameters.AddWithValue("@CustomOrdering", dtPreStatus.Rows[0]["CustomOrdering"].ToString());
                cmd.Parameters.AddWithValue("@OverTime", dtPreStatus.Rows[0]["OverTime"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpDutyType", dtPreStatus.Rows[0]["PreEmpDutyType"].ToString());
                cmd.Parameters.AddWithValue("@EmpDutyType", dtPreStatus.Rows[0]["EmpDutyType"].ToString());
                cmd.Parameters.AddWithValue("@EmpJoinigSalary", dtPreStatus.Rows[0]["EmpJoinigSalary"].ToString());
                cmd.Parameters.AddWithValue("@DormitoryRent", dtPreStatus.Rows[0]["DormitoryRent"].ToString());
                cmd.Parameters.AddWithValue("@PreIncomeTax", dtPreStatus.Rows[0]["PreIncomeTax"].ToString());
                cmd.Parameters.AddWithValue("@IncomeTax",txtNewTDS.Text.Trim());
                int i = (int)cmd.ExecuteNonQuery();
                if (i > 0)
                {

                    lblMessage.InnerText = "success->Successfully Salary Incremented";

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadSalaryInfo();
                }
            }
              catch (Exception ex)
            {
                lblMessage.InnerText = "error->"+ex.Message;
            }
        }
        private void updateSalaryIncrement()
        {
            try
            {
                string getMedical = "0";
                string getBasic = "0";
                string getHouseRent = "0";
                string getConveyance = "0";

                if (hfSalaryType.Value == "Scall")
                {
                    getMedical = (txtNewMedical.Text.Trim().Length > 0) ? txtNewMedical.Text : txtPresentMedical.Text;
                    // getBasic = ((txtPresentBasic.Text.Trim().Length == 0) || (txtPresentBasic.Text.Trim().Equals("0"))) ? txtPresentBasic.Text : txtNewBasic.Text;
                    getHouseRent = (txtNewHouseRent.Text.Trim().Length > 0) ? txtNewHouseRent.Text : txtPresentHouseRent.Text;
                    getConveyance = (txtNewConveyance.Text.Trim().Length > 0) ? txtNewConveyance.Text : txtPresentConveyance.Text;
                }
                string getNewGross = (txtNewGross.Text.Trim().Length > 0) ? txtNewGross.Text : txtPresentGrossSalary.Text;
                string getLastIncrement=(txtIncrementAmount.Text.Trim().Length>0)?txtIncrementAmount.Text:txtLastIncrementAmount.Text;
                SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update Personnel_EmpCurrentStatus set  EmpPresentSalary=@EmpPresentSalary,IncrementAmount=@IncrementAmount,MedicalAllownce=@MedicalAllownce,BasicSalary=@BasicSalary,HouseRent=@HouseRent,ConvenceAllownce=@ConvenceAllownce,HolidayAllownce=@HolidayAllownce,TiffinAllownce=@TiffinAllownce,NightAllownce=@NightAllownce,AttendanceBonus=@AttendanceBonus,EffectiveDate=@EffectiveDate,OrderRefNo=@OrderRefNo,OrderRefDate=@OrderRefDate,Remarks=@Remarks where SN="+hfEmpSN.Value+"",sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpPresentSalary",getNewGross);
                cmd.Parameters.AddWithValue("@IncrementAmount",getLastIncrement);
                cmd.Parameters.AddWithValue("@MedicalAllownce",getMedical);
                cmd.Parameters.AddWithValue("@BasicSalary", txtNewBasic.Text.Trim());
                cmd.Parameters.AddWithValue("@HouseRent",getHouseRent);
                cmd.Parameters.AddWithValue("@ConvenceAllownce",getConveyance);
                cmd.Parameters.AddWithValue("@HolidayAllownce", "0");
                cmd.Parameters.AddWithValue("@TiffinAllownce", "0");
                cmd.Parameters.AddWithValue("@NightAllownce", "0");
                cmd.Parameters.AddWithValue("@AttendanceBonus", "0");         
                cmd.Parameters.AddWithValue("@EffectiveDate", convertDateTime.getCertainCulture(txtEffectiveFrom.Text.Trim()));
                cmd.Parameters.AddWithValue("@OrderRefNo", txtIncrementOrderRefNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@OrderRefDate", convertDateTime.getCertainCulture(txtIncrementOrderRefDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());

                int result = cmd.ExecuteNonQuery();

                if (result==1)
                {
                    lblMessage.InnerText = "success->Successfully Updated";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadSalaryInfo();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void loadSalaryInfo()
        {
            try
            {
                  
                dt = new DataTable();
                //sqlDB.fillDataTable("Select SN,EmpId From  v_EmployeeDetails where TypeOfChange='i' and ActiveSalary=0 and CompanyId='" + ViewState["__CompanyId__"] + "'  Group by EmpId", dt);
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

                sqlDB.fillDataTable("select SN,EmpId,CompanyName,EmpCardNo, EmpPresentSalary,SalaryType,Convert(varchar(11),DateofUpdate,105) as DateofUpdate,EmpType,EffectiveMonth,IncrementAmount  from v_Personnel_EmpCurrentStatus where TypeOfChange='i' and ActiveSalary=0 and CompanyId='" + ViewState["__CompanyId__"] + "' order by EffectiveMonth desc", dt = new DataTable());
                divSalaryIncrementList.DataSource = dt;
                divSalaryIncrementList.DataBind();
                if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    divSalaryIncrementList.Columns[8].Visible = false;

            }
            catch { }       
        }   
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "getDeleteMessage();", true);
                //if (hfDeleteStatus.Value.ToString().Equals("1"))
                //{
                   deletePromotion("","");
                //}
                //else loadSalaryInfo();
            }
            catch { }
        }

        private void deletePromotion(string SN,string EmpId)
        {
            try
            {
                if (SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpCurrentStatus", "SN", SN, sqlDB.connection) == true)
                {
                    SqlCommand upIsActive = new SqlCommand("Update Personnel_EmpCurrentStatus set IsActive=1 where SN=(Select Max(SN) as SN From Personnel_EmpCurrentStatus where EmpId='"+EmpId+"')", sqlDB.connection);
                    upIsActive.ExecuteNonQuery();
                    lblMessage.InnerText = "success->Successfully Deleted";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadSalaryInfo();
                }
            }
            catch { }
        }

        protected void btnIncrementInfo_Click(object sender, EventArgs e)
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
                    CompanyId = ViewState["__CompanyId__"].ToString();
                }
                DataTable dtPromotionInfo = new DataTable();
                sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,PreIncrementAmount,IncrementAmount,EmpType,DptName,DsgName,PreEmpSalary,EmpPresentSalary,PreBasicSalary,BasicSalary,TechnicalAllownce,PreConvenceAllownce,ConvenceAllownce,PreMedicalAllownce,MedicalAllownce,Address,CompanyName From v_Promotion_Increment where CompanyId='" + CompanyId + "' AND EmpId in(select EmpId from Personnel_EmpCurrentStatus where SN='" + ddlEmpCardNo.SelectedValue + "') and TypeOfChange='i'", dtPromotionInfo);
                Session["__IncrementInfo__"] = dtPromotionInfo;
                if (dtPromotionInfo.Rows.Count > 0)
                {

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IncrementInfo');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    lblMessage.InnerText = "warning->No Incrment ";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }               

            }
            catch { }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, ddlCompany.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void divSalaryIncrementList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "deleterow")
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    deletePromotion(divSalaryIncrementList.DataKeys[rIndex].Values[0].ToString(), divSalaryIncrementList.DataKeys[rIndex].Values[1].ToString());
                }
            }
            catch { }
        }

        protected void divSalaryIncrementList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            loadSalaryInfo();
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void btnComplain_Click(object sender, EventArgs e)
        {
            Session["__ModuleType__"] = "Personnel";
            Session["__forCompose__"] = "No";
            Session["__PreviousPage__"] = Request.ServerVariables["HTTP_REFERER"].ToString();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            Response.Redirect("/mail/complain.aspx");
        }

        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNewBasic.Text = "";
            txtNewConveyance.Text = "";
            txtNewFoodAllowance.Text = "";
            txtNewGross.Text = "";
            txtNewHouseRent.Text = "";
            txtNewMedical.Text = "";
            lblbasicnew.Text = "";
            lblconveyancenew.Text = "";
            lblfoodnew.Text = "";
            lblhouserentnew.Text = "";
            lblmedicalnew.Text = "";
            txtIncrementAmount.Text = "";
            txtNewTransportOthers.Text = "";
            txtNewPF.Text = "";
            findLastSalaryIncrement();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
        }

       

        
        
    }
}