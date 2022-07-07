using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System.Data.SqlClient;

namespace SigmaERP.classes
{
    public  class Tax_Calculation
    {
       

      static  float BasicSalary_60;
      static float BasicSalary_68;
      static float BasicSlaryYearly_60;
      static float PresentSalaryYearly;
      static float OthersAllowanceYearly;
      static float TaxableOthersIncome;
      static float _TaxableTotalIncome;
      static float TotalTaxAmount;
      static float RebateAmount;
      static float NetPayableTax;
      static float NetPayableTaxPerMonth;

      static float HouseRant;
      static float HouseRantTaxFree;
      static float Medical;
      static float MedicalTexFree;
      static float Conveyance;
      static float ConveyanceTaxFree;
      static float _EmpEmprContribution;
      static float _MaximumInvastment;
      static float _TotalInvastment;
      static float _RebatableInvastment;

      static float _preTaxPerMonth = 0;
      static float _FirstPartEmpPresentSalary=0;
      static string _TaxId = "";
      static string _EmpId = "";
    //  static DataTable dtTaxSlapInfo ;
    //  static DataTable dtRebateSlapInfo;
      static DataTable dtTaxSlap;
      static DataTable dtRebateSlap;


      public static void generateTaxCalculation(string UserId, string TaxId, string TaxYears, string TaxOrder,string FromMonth,string ToMonth, string CompanyId, string EmpId, float EmpPresenSalary, bool isPF, int BasicPerForTax, int BasicPer,int pfPer,int MedicalPer, DataTable dtTaxSlapInfo, DataTable dtRebateSlapInfo, DataTable dtTaxFreeInfo,float IncomTax,int MinimumTax) 
        {
            _EmpId = EmpId;
            _TaxId = TaxId;
            float Bonus = 0;
            float EL_Amount = 0;
            float PF_Amount = 0;   
            SqlCommand cmd = new SqlCommand("delete VatTax_IncomeTax where EmpId='" + EmpId + "' and TaxId=" + TaxId + "", sqlDB.connection);
            cmd.ExecuteNonQuery();

            if (int.Parse(TaxOrder)==0)//Final and Actual calculation
            {
      //         EmpPresenSalary = 83000;
                DataTable dtPreTax = new DataTable();
                sqlDB.fillDataTable("select OrderNo , PerMonthTax ,PresentSalary from v_VatTax_IncomeTax where TaxYears='" + TaxYears + "' and EmpId='" + EmpId + "'  order by OrderNo ", dtPreTax);
                if (dtPreTax.Rows.Count > 0) // 2nd and Final Calculation
                {
                    _preTaxPerMonth = float.Parse(dtPreTax.Rows[0]["PerMonthTax"].ToString());
                    _FirstPartEmpPresentSalary = float.Parse(dtPreTax.Rows[0]["PresentSalary"].ToString()) / 2;
                    PresentSalaryYearly = (EmpPresenSalary * 6) + _FirstPartEmpPresentSalary;
                    BasicSlaryYearly_60 = (PresentSalaryYearly * BasicPerForTax) / 100;
                    EL_Amount = ELAmount(EmpId, TaxYears);
                    Bonus = BonusAmount(EmpId,FromMonth,ToMonth);
                 //   EL_Amount = 26973;
                  //  Bonus = 104040;
                    if (isPF)
                        PF_Amount = (((PresentSalaryYearly * BasicPer) / 100) * pfPer) / 100; //PFAmount(EmpId);
                    OthersAllowanceYearly = ((PresentSalaryYearly * (100 - BasicPerForTax)) / 100); // Yearly Others Allowance . 40 % of Gross
                    TaxableOthersIncome = OthersIncome(dtTaxFreeInfo, MedicalPer);
                    _TaxableTotalIncome = BasicSlaryYearly_60 + Bonus + EL_Amount + PF_Amount + TaxableOthersIncome;
                    TotalTaxAmount = TotalTax(dtTaxSlapInfo);
                    if (TotalTaxAmount > 0)
                    {

                        _EmpEmprContribution = PF_Amount * 2;
                        _TotalInvastment = _EmpEmprContribution + Investment(EmpId,TaxYears);
                        _MaximumInvastment = (_TaxableTotalIncome - PF_Amount) * 25 / 100;
                        if (_TotalInvastment < _MaximumInvastment)
                            _RebatableInvastment = _TotalInvastment;
                        else
                            _RebatableInvastment = _MaximumInvastment;
                        if (_RebatableInvastment > 15000000)
                            _RebatableInvastment = 15000000;
                        RebateAmount = Rebate(dtRebateSlapInfo);
                        NetPayableTax = TotalTaxAmount - RebateAmount;
                        if (NetPayableTax < MinimumTax)
                            NetPayableTax = MinimumTax;
                        //----------------------------------------
                        DataTable dtPaidTax = new DataTable();

                        sqlDB.fillDataTable("select isnull(sum(TaxAmount),0) PaidTax,isnull(count(EmpId),0) PaidMonthNumber from VatTax_IncomeTaxDetailsLog where EmpId='" + EmpId + "' and isPaid=1 and TaxYears='" + TaxYears + "'", dtPaidTax);
                     
                        //-------------------------------------
                       // NetPayableTaxPerMonth = (NetPayableTax - (_preTaxPerMonth * 6)) / 6;
                        NetPayableTaxPerMonth = (NetPayableTax - float.Parse(dtPaidTax.Rows[0]["PaidTax"].ToString())) / (12 - int.Parse(dtPaidTax.Rows[0]["PaidMonthNumber"].ToString()));
                    }
                    else
                    {
                        _RebatableInvastment = 0;
                        _EmpEmprContribution = 0;
                        RebateAmount = 0;
                        NetPayableTax = 0;
                        NetPayableTaxPerMonth = 0;
                    }
                }
                else // 1st and Final Calculation
                {
                    PresentSalaryYearly = EmpPresenSalary * 12;
                    BasicSlaryYearly_60 = (PresentSalaryYearly * BasicPerForTax) / 100;
                    EL_Amount = ELAmount(EmpId, TaxYears);
                    Bonus = BonusAmount(EmpId,FromMonth,ToMonth);
                    if (isPF)
                        PF_Amount = PFAmount(EmpId);
                    OthersAllowanceYearly = ((PresentSalaryYearly * (100 - BasicPerForTax)) / 100); // Yearly Others Allowance . 40 % of Gross
                    TaxableOthersIncome = OthersIncome(dtTaxFreeInfo, MedicalPer);
                    _TaxableTotalIncome = BasicSlaryYearly_60 + Bonus + EL_Amount + PF_Amount + TaxableOthersIncome;
                    TotalTaxAmount = TotalTax(dtTaxSlapInfo);
                    if (TotalTaxAmount > 0)
                    {
                        _EmpEmprContribution = PF_Amount * 2;
                        _TotalInvastment = _EmpEmprContribution + Investment(EmpId, TaxYears);
                        _MaximumInvastment = (_TaxableTotalIncome - PF_Amount) * 25 / 100;

                        if (_TotalInvastment < _MaximumInvastment)
                            _RebatableInvastment = _TotalInvastment;
                        else 
                            _RebatableInvastment = _MaximumInvastment;
                        if (_RebatableInvastment > 15000000)
                            _RebatableInvastment = 15000000;
                        RebateAmount = Rebate(dtRebateSlapInfo);
                        NetPayableTax = TotalTaxAmount - RebateAmount;
                        if (NetPayableTax < MinimumTax)
                            NetPayableTax = MinimumTax;
                        NetPayableTaxPerMonth = NetPayableTax / 12;
                    }
                    else
                    {
                        _RebatableInvastment = 0;
                        _EmpEmprContribution = 0;
                        RebateAmount = 0;
                        NetPayableTax = 0;
                        NetPayableTaxPerMonth = 0;
                    }
                }
               
            }
            else // First and Probable calculation
            {
                //EmpPresenSalary = 70000;
                BasicSalary_68 = (EmpPresenSalary * BasicPer) / 100;
                BasicSalary_60 = (EmpPresenSalary * BasicPerForTax) / 100;
                PresentSalaryYearly = EmpPresenSalary * 12;
                BasicSlaryYearly_60 = BasicSalary_60 * 12; // Yearly Basic Salary . 60 % of Gross
                Bonus = BasicSalary_68 * 2;
                if (isPF)
                    PF_Amount = ((BasicSalary_68 * pfPer) / 100) * 12;

                OthersAllowanceYearly = ((PresentSalaryYearly * (100 - BasicPerForTax)) / 100); // Yearly Others Allowance . 40 % of Gross
                TaxableOthersIncome = OthersIncome(dtTaxFreeInfo,MedicalPer);
                _TaxableTotalIncome = BasicSlaryYearly_60 + Bonus + EL_Amount + PF_Amount + TaxableOthersIncome;
                TotalTaxAmount = TotalTax(dtTaxSlapInfo);
                if (TotalTaxAmount > 0)
                {
                    _EmpEmprContribution = PF_Amount * 2;
                    _TotalInvastment = _EmpEmprContribution + Investment(EmpId, TaxYears);
                    _MaximumInvastment = (_TaxableTotalIncome - PF_Amount) * 25 / 100;
                    if (_TotalInvastment < _MaximumInvastment)
                        _RebatableInvastment = _TotalInvastment;
                    else
                        _RebatableInvastment = _MaximumInvastment;

                    if (_RebatableInvastment > 15000000)
                        _RebatableInvastment = 15000000;
                    RebateAmount = Rebate(dtRebateSlapInfo);
                    NetPayableTax = TotalTaxAmount - RebateAmount;
                    if (NetPayableTax < MinimumTax)
                        NetPayableTax = MinimumTax;
                    NetPayableTaxPerMonth = NetPayableTax / 12;
                }
                else
                {
                    _RebatableInvastment = 0;
                    _EmpEmprContribution = 0;
                    RebateAmount = 0;
                    NetPayableTax = 0;
                    NetPayableTaxPerMonth = 0;
                }
                DataTable dtpretax=new DataTable();
                sqlDB.fillDataTable(" select  ProfitTax from Payroll_MonthlySalarySheet  where YearMonth='2016-12-01' and EmpId='" + EmpId + "'", dtpretax);
                try 
                {
                    NetPayableTaxPerMonth = float.Parse(dtpretax.Rows[0]["ProfitTax"].ToString());
                }
                catch { NetPayableTaxPerMonth = 0; }
                
               
               
            }    
           
            string[] getColumns = {"TaxId", "EmpId", "CompanyId", "PresentSalary", "BasicSalary", "Bonus", "PF_Amount", "EL_Amount", "OthersIncome",
                                        "Total_Taxable_Income", "OthersAllowance","Conveyance", "ConveyanceTaxFree", "HouseRent", "HouseRentTaxFree","Madical",
                                        "MadicalTaxFree", "TotalTax","Rebatable","NetPayableTax","PerMonthTax","EmpEmprContribution","GenerateDate","UserId","MaxInvestmentAmount","TotalInvestmentAmount","RebatableInvestment"};

            string[] getValues = {TaxId, EmpId, CompanyId, PresentSalaryYearly.ToString(), BasicSlaryYearly_60.ToString(),Bonus.ToString(),PF_Amount.ToString(),EL_Amount.ToString(),
                                 TaxableOthersIncome.ToString(),_TaxableTotalIncome.ToString(),OthersAllowanceYearly.ToString(),Conveyance.ToString(),ConveyanceTaxFree.ToString(),
                                 HouseRant.ToString(),HouseRantTaxFree.ToString(),Medical.ToString(),MedicalTexFree.ToString(),TotalTaxAmount.ToString(),RebateAmount.ToString(),
                                 NetPayableTax.ToString(),Math.Round(NetPayableTaxPerMonth,0).ToString(),_EmpEmprContribution.ToString(),DateTime.Now.ToString(),UserId,_MaximumInvastment.ToString(),_TotalInvastment.ToString(),_RebatableInvastment.ToString()};
            SQLOperation.forSaveValue("VatTax_IncomeTax", getColumns, getValues, sqlDB.connection);
            //NetPayableTaxPerMonth = IncomTax;
            saveIncomeTaxDetailsLog(EmpId,Math.Round(NetPayableTaxPerMonth,0).ToString(), FromMonth,ToMonth,TaxYears);
            saveTaxCalculation();
            saveRebateCalculation();
        }
      private static int Investment(string EmpId,string TaxYears) 
      {
          try {
              DataTable dt = new DataTable();
              sqlDB.fillDataTable("select (LifeInsurPremium+ContrDepositPensionScheme+InvstInApprSavingsCertificate+InvstInApprDebentureOrDebentureStock_StockOrShares+ContrPFWhichPFAct1925Applies+ContrSuperAnnuationFund+ContrBenevolentFundAndGroupInsurPremium+ContrZakatFund+Others) as Investment from VatTax_EmpInvestment where InvstYear='" + TaxYears + "' and EmpId='" + EmpId + "'", dt);
              return int.Parse(dt.Rows[0]["Investment"].ToString());
           
          }
          catch { return 0; }
      }
      private static void saveIncomeTaxDetailsLog(string EmpId, string TaxAmount,string FromMonth,string ToMonth,string TaxYears)
    {

      
           
            DateTime FMonth = DateTime.Parse(FromMonth);
            DateTime TMonth = DateTime.Parse(ToMonth);
            DateTime i;           
            for (i = FMonth; i <= TMonth; i = i.AddMonths(1))
            {
                  try
                 {


                     string[] getColumns = { "EmpId", "Month", "TaxAmount", "TaxYears", "isPaid" };
                string[] getValues = { EmpId, i.ToString("yyyy-MM-dd"), TaxAmount,TaxYears, "0" };
                SqlCommand cmd = new SqlCommand("delete VatTax_IncomeTaxDetailsLog where EmpId='"+EmpId+"' and Month='"+i.ToString()+"' and isPaid=0", sqlDB.connection);
                cmd.ExecuteNonQuery();
                SQLOperation.forSaveValue("VatTax_IncomeTaxDetailsLog", getColumns, getValues, sqlDB.connection);
                 }
                  catch { }
            }
           
      
    }
      private static float OthersIncome(DataTable dtTaxFreeInfo,int MedicalPer) 
        {

             HouseRant = BasicSlaryYearly_60 / 2;
             HouseRantTaxFree = float.Parse(dtTaxFreeInfo.Rows[0]["HouseRent"].ToString());
            if (HouseRant < HouseRantTaxFree)
                HouseRantTaxFree = HouseRant;

            Medical = (BasicSlaryYearly_60 * MedicalPer) / 100;
             MedicalTexFree = float.Parse(dtTaxFreeInfo.Rows[0]["MedicalAllownce"].ToString());
            if (Medical < MedicalTexFree)
                MedicalTexFree = Medical;

             Conveyance = OthersAllowanceYearly - (HouseRant + Medical);
             ConveyanceTaxFree = float.Parse(dtTaxFreeInfo.Rows[0]["ConvenceAllownce"].ToString());
            if (Conveyance < ConveyanceTaxFree)
                ConveyanceTaxFree = Conveyance;
            return OthersAllowanceYearly - (HouseRantTaxFree + MedicalTexFree + ConveyanceTaxFree);
        }

        private static float TotalTax(DataTable Slap)
        {

            //float TaxableIncome = 9000000;           
           //   float[] Slab = { 250000, 400000, 500000, 600000, 3000000, 0 };
           // float[] TaxPer = { 0, 10, 15, 20, 25, 30 };
            float TotalTax = 0;
            float tax = 0;
            float TaxableTotalIncome = _TaxableTotalIncome;
            dtTaxSlap = new DataTable();            
            dtTaxSlap.Columns.Add("SlabName", typeof(string));
            dtTaxSlap.Columns.Add("Amount", typeof(string));
            dtTaxSlap.Columns.Add("Rate", typeof(string));
            dtTaxSlap.Columns.Add("TaxableAmount", typeof(string));
            dtTaxSlap.Columns.Add("OrderNo", typeof(string));
            string SlapName = "First";
            for (byte i = 0; i < Slap.Rows.Count; i++)
            {
                if (i > 0)
                    SlapName = "Next";
                if (i == Slap.Rows.Count - 1)
                {
                    tax = (TaxableTotalIncome * float.Parse(Slap.Rows[i]["IncomeTaxRate"].ToString())) / 100;
                    TotalTax += tax;

                    dtTaxSlap.Rows.Add(SlapName, TaxableTotalIncome.ToString(), Slap.Rows[i]["IncomeTaxRate"].ToString(), tax.ToString(), i.ToString());

                    
                    break;
                }
                if (TaxableTotalIncome <= float.Parse(Slap.Rows[i]["ToTaka"].ToString()))
                {
                    tax = (TaxableTotalIncome * float.Parse(Slap.Rows[i]["IncomeTaxRate"].ToString())) / 100;
                    TotalTax += tax;
                    dtTaxSlap.Rows.Add(SlapName, TaxableTotalIncome.ToString(), Slap.Rows[i]["IncomeTaxRate"].ToString(), tax.ToString(), i.ToString());
                    break;
                }
                else
                {
                    tax = (float.Parse(Slap.Rows[i]["ToTaka"].ToString()) * float.Parse(Slap.Rows[i]["IncomeTaxRate"].ToString())) / 100;
                    TotalTax += tax;
                    TaxableTotalIncome = TaxableTotalIncome - float.Parse(Slap.Rows[i]["ToTaka"].ToString());
                    dtTaxSlap.Rows.Add(SlapName, Slap.Rows[i]["ToTaka"].ToString(), Slap.Rows[i]["IncomeTaxRate"].ToString(), tax.ToString(), i.ToString());
                   
                }
                if (TaxableTotalIncome <= 0)
                    break;
                
            }
            return TotalTax;
        }
        
        private static float Rebate(DataTable Slap)
        {
         //   float TaxableIncome = 9000000;
         //   float[] Slab = { 250000, 400000, 500000, 600000, 3000000, 0 };
         //   float[] TaxPer = { 0, 10, 15, 20, 25, 30 };
            float TotalTax = 0;
            float tax = 0;
            float RebatableInvastment = _RebatableInvastment;
            dtRebateSlap = new DataTable();
            dtRebateSlap.Columns.Add("SlabName", typeof(string));
            dtRebateSlap.Columns.Add("Amount", typeof(string));
            dtRebateSlap.Columns.Add("Rate", typeof(string));
            dtRebateSlap.Columns.Add("RebatableAmount", typeof(string));
            dtRebateSlap.Columns.Add("OrderNo", typeof(string));
            string SlapName = "First";
            for (byte i = 0; i < Slap.Rows.Count; i++)
            {
                if (i > 0)
                    SlapName = "Next";
                if (i == Slap.Rows.Count - 1)
                {
                    tax = (RebatableInvastment * float.Parse(Slap.Rows[i]["IncomeTaxRate"].ToString())) / 100;
                    TotalTax += tax;
                    dtRebateSlap.Rows.Add(SlapName, RebatableInvastment.ToString(), Slap.Rows[i]["IncomeTaxRate"].ToString(), tax.ToString(), i.ToString());
                    break;
                }
                if (RebatableInvastment <= float.Parse(Slap.Rows[i]["ToTaka"].ToString()))
                {
                    tax = (RebatableInvastment * float.Parse(Slap.Rows[i]["IncomeTaxRate"].ToString())) / 100;
                    TotalTax += tax;
                    dtRebateSlap.Rows.Add(SlapName, RebatableInvastment.ToString(), Slap.Rows[i]["IncomeTaxRate"].ToString(), tax.ToString(), i.ToString());
                   
                    break;
                }
                else
                {
                    tax = (float.Parse(Slap.Rows[i]["ToTaka"].ToString()) * float.Parse(Slap.Rows[i]["IncomeTaxRate"].ToString())) / 100;
                    TotalTax += tax;
                    RebatableInvastment = RebatableInvastment - float.Parse(Slap.Rows[i]["ToTaka"].ToString());
                    dtRebateSlap.Rows.Add(SlapName, Slap.Rows[i]["ToTaka"].ToString(), Slap.Rows[i]["IncomeTaxRate"].ToString(), tax.ToString(), i.ToString());
                   
                }

                if (RebatableInvastment <= 0)
                    break;
            }
            
            return TotalTax;
        }
        private static void saveTaxCalculation()
        {
            try
            {
                if (dtTaxSlap.Rows.Count > 0)
                {
                    for (byte i = 0; i < dtTaxSlap.Rows.Count; i++)
                    {
                        string[] getColumns = { "TaxId", "EmpId", "SlabName", "Amount", "Rate", "TaxableAmount", "OrderNo" };

                        string[] getValues = { _TaxId,_EmpId, dtTaxSlap.Rows[i]["SlabName"].ToString(), 
                                     dtTaxSlap.Rows[i]["Amount"].ToString(), dtTaxSlap.Rows[i]["Rate"].ToString(), dtTaxSlap.Rows[i]["TaxableAmount"].ToString(), 
                                     dtTaxSlap.Rows[i]["OrderNo"].ToString() };
                        SQLOperation.forSaveValue("VatTax_TaxCalculation", getColumns, getValues, sqlDB.connection);
                    }
                }
            }
            catch { }


        }
        private static void saveRebateCalculation()
        {
            try
            {
                if (dtRebateSlap.Rows.Count > 0)
                {
                    for (byte i = 0; i < dtTaxSlap.Rows.Count; i++)
                    {
                        string[] getColumns = { "TaxId", "EmpId", "SlabName", "Amount", "Rate", "RebatableAmount", "OrderNo" };

                        string[] getValues = { _TaxId,_EmpId, dtRebateSlap.Rows[i]["SlabName"].ToString(),dtRebateSlap.Rows[i]["Amount"].ToString(), 
                                                 dtRebateSlap.Rows[i]["Rate"].ToString(), dtRebateSlap.Rows[i]["RebatableAmount"].ToString(), 
                                     dtRebateSlap.Rows[i]["OrderNo"].ToString() };
                        SQLOperation.forSaveValue("VatTax_RebateCalculation", getColumns, getValues, sqlDB.connection);
                    }
                }
            }
            catch { }


        }
        private static float BonusAmount(string EmpId,string FromMonth,string ToMonth) 
        {
            try
            {
                ToMonth = DateTime.Parse(ToMonth).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select sum(BonusAmount) as BonusAmount from v_Payroll_YearlyBonusSheet where EmpId='" + EmpId + "' and CalculationDate>='" + FromMonth + "' and CalculationDate<='" + ToMonth + "'", dt);
                return float.Parse(dt.Rows[0]["BonusAmount"].ToString());
            }
            catch { return 0; }
         
        }
        private static float PFAmount(string EmpId)
        {
            try
            {

               // DataTable dt = new DataTable();
               // sqlDB.fillDataTable("select sum(BonusAmount) as BonusAmount from Payroll_YearlyBonusSheet where EmpId='" + EmpId + "' and GenerateDate>='2016-07-01' and GenerateDate<='2017-06-30'", dt);
               // return float.Parse(dt.Rows[0]["BonusAmount"].ToString());
                return (((BasicSlaryYearly_60/12) * 10) / 100) * 12;
            }
            catch { return 0; }

        }
        private static float ELAmount(string EmpId,string TaxYear)
        {
            try
            {
                string[] Year = TaxYear.Split('-');
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select NetTotal from Leave_YearlyEarnLeaveGeneration where EmpId='" + EmpId + "' and Year(EarnLeavePerviousStartYear)='" + Year[0] + "'", dt);
                if(dt.Rows.Count>0)
                return float.Parse(dt.Rows[0]["NetTotal"].ToString());
                else
                return 0;
            }
            catch { return 0; }

        }
        
      

        
    }
    

    

}