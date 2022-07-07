using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class pay_slip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();

                this.btnaddall.Click += new System.EventHandler(this.btnaddall_Click);
                this.btnadditem.Click += new System.EventHandler(this.btnadditem_Click);
                this.btnremoveitem.Click += new System.EventHandler(this.btnremoveitem_Click);
                this.btnremoveall.Click += new System.EventHandler(this.btnremoveall_Click);

                this.Load += new System.EventHandler(this.Page_Load);

                if (!IsPostBack)
                {
                    setPrivilege();
                    loadMonthId();
                    loadDevision();
                }

            }
            catch { }
        }

        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='pay_slip.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreview.CssClass = "";
                            btnPreview.Enabled = false;
                        }
                    }
                }
            }
            catch { }
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

        private void btnaddall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstEmployees, lstSelectedEmployees);
        }

        private void btnadditem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstEmployees, lstSelectedEmployees);
        }

        private void btnremoveitem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstSelectedEmployees, lstEmployees);
        }

        private void btnremoveall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstSelectedEmployees, lstEmployees);
        }

        private void loadMonthId()
        {
            try
            {
               DataTable dt=new DataTable ();
               sqlDB.fillDataTable("select distinct (Convert(nvarchar(50),v_MonthlySalarySheet.Year)+'-'+v_MonthlySalarySheet.Month) as Month,MonthYear,Year from v_MonthlySalarySheet order by Year desc, MonthYear ", dt);
               ddlMonthID.DataTextField = "Month";
               ddlMonthID.DataValueField = "Month";
               ddlMonthID.DataSource = dt;
               ddlMonthID.DataBind();
               ddlMonthID.Items.Insert(0,new ListItem (" "," "));
            }
            catch { }
        }

        private void loadDevision() //Load Devision 
        {
            try
            {
                classes.commonTask.loadDivision(ddlDivision);
                //ddlDivision.Items.Clear();
                //sqlDB.bindDropDownList("Select distinct DName From v_tblAttendanceRecord", "DName", ddlDivision);
                //ddlDivision.Items.Add(" ");
            }
            catch { }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lstEmployees.Items.Clear();
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select distinct DptName From v_tblAttendanceRecord Where DName='" + ddlDivision.SelectedItem.Text + "' ", dt);
                lstEmployees.DataTextField = "DptName";
                lstEmployees.DataValueField = "DptName";
                lstEmployees.DataSource = dt;
                lstEmployees.DataBind();
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string setPredicate = "";
                for (byte b = 0; b < lstSelectedEmployees.Items.Count; b++)
                {
                    if (b == 0 && b == lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelectedEmployees.Items[b].Text + "')";
                    }
                    else if (b == 0 && b != lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelectedEmployees.Items[b].Text + "'";
                    }
                    else if (b != 0 && b == lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelectedEmployees.Items[b].Text + "')";
                    }
                    else setPredicate += ",'" + lstSelectedEmployees.Items[b].Text + "'";
                }

                DataTable dt = new DataTable();
                if (ddlType.SelectedItem.Text == "Individual") sqlDB.fillDataTable("Select DptId,EmpNameBn,DsgNameBn,EmpName,EmpCardNo,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate ,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,GrdNameBangla,PayableDays,HouseRent,AttendanceBonus,IsSeperationGeneration From v_MonthlySalarySheet Where EmpCardNo='" + ddlIndividualEmp.SelectedItem.Text + "' and Month='" + ddlMonthID.SelectedItem.Text.Substring(5, 2) + "' and Year='" + ddlMonthID.SelectedItem.Text.Substring(0, 4) + "' and EmpType='" + rdoEmpType.SelectedItem.ToString() + "' IsSeperationGeneration='0'   Group By DptId,EmpNameBn,DsgNameBn,EmpName,EmpCardNo,EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,GrdNameBangla,PayableDays,HouseRent,AttendanceBonus,IsSeperationGeneration  Order By DptId,LnCode,EmpCardNo ", dt);
                else sqlDB.fillDataTable("Select DptId,EmpNameBn,DsgNameBn,EmpName,EmpCardNo,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,GrdNameBangla,PayableDays,HouseRent,AttendanceBonus,IsSeperationGeneration From v_MonthlySalarySheet Where  DptName " + setPredicate + " and Month='" + ddlMonthID.SelectedItem.Text.Substring(5, 2) + "' and Year='" + ddlMonthID.SelectedItem.Text.Substring(0, 4) + "' and EmpType='" + ddlType.SelectedItem.Text + "' and IsSeperationGeneration='0'  Group By DptId,EmpNameBn,DsgNameBn,EmpName,EmpCardNo,EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,GrdNameBangla,PayableDays,HouseRent,AttendanceBonus,IsSeperationGeneration  Order By DptId,LnCode,EmpCardNo ", dt);
                Session["__dtJobCard__"] = dt;

                
                if (rbLanguage.SelectedValue.ToString() == "0") Session["__Language__"] = "Bangla"; // 0=Bangla 1= English

                else Session["__Language__"] = "English";

                if (dt.Rows.Count > 0) ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=PaySlip-"+ddlMonthID.Text.ToString()+"');", true);  //Open New Tab for Sever side code
            }
            catch { }   
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                if (ddlType.SelectedItem.Text == "Individual")
                {
                    ddlIndividualEmp.Enabled = true;
                  //  ddlMonthID.Enabled = false;
                    fldEmpType.Visible = true;
                }
                else
                {
                    ddlIndividualEmp.Enabled = false;
                 //   ddlMonthID.Enabled = true;
                    fldEmpType.Visible = false;
                }
                 
            }
            catch { }
        }

        protected void rdoEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoEmpType.SelectedIndex == 0)
                {
                    DataTable dt = new DataTable();
                    sqlDB.bindDropDownList("Select EmpCardNo From v_MonthlySalarySheet Where EmpType='Worker' order by EmpCardNo ", "EmpCardNo", ddlIndividualEmp);
                }
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.bindDropDownList("Select EmpCardNo From v_MonthlySalarySheet Where EmpType='Staff' order by EmpCardNo ", "EmpCardNo", ddlIndividualEmp);
                }
            }
            catch { }
        }

        protected void btnPreview2_Click(object sender, EventArgs e)
        {
            try
            {
                string setPredicate = "";
                for (byte b = 0; b < lstSelectedEmployees.Items.Count; b++)
                {
                    if (b == 0 && b == lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelectedEmployees.Items[b].Text + "')";
                    }
                    else if (b == 0 && b != lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelectedEmployees.Items[b].Text + "'";
                    }
                    else if (b != 0 && b == lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelectedEmployees.Items[b].Text + "')";
                    }
                    else setPredicate += ",'" + lstSelectedEmployees.Items[b].Text + "'";
                }

                DataTable dt = new DataTable();
                if (ddlType.SelectedItem.Text == "Individual") sqlDB.fillDataTable("Select EmpName,EmpCardNo,DptId,convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate ,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,TotalSalaryWithAllOT,IsSeperationGeneration From v_MonthlySalarySheet Where EmpCardNo='" + ddlIndividualEmp.SelectedItem.Text + "' and IsSeperationGeneration='0'  Group By EmpName,EmpCardNo,EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,TotalSalaryWithAllOT,dptId,IsSeperationGeneration  Order By dptId,LnCode,EmpCardNo ", dt);
                else sqlDB.fillDataTable("Select EmpName,EmpCardNo,dptId,convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,TotalSalaryWithAllOT,IsSeperationGeneration From v_MonthlySalarySheet Where  DptName " + setPredicate + " and Month='" + ddlMonthID.SelectedItem.Text.Substring(5, 2) + "' and Year='" + ddlMonthID.SelectedItem.Text.Substring(0, 4) + "' and EmpType='" + ddlType.SelectedItem.Text + "' and IsSeperationGeneration='0'  Group By EmpName,EmpCardNo,EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,AbsentDay,PresentDay,PayableDays,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,LunchAllowance,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,TotalSalaryWithAllOT,dptId,IsSeperationGeneration  Order By dptId,LnCode,EmpCardNo ", dt);
                Session["__dtJobCard__"] = dt;

                if (dt.Rows.Count > 0) ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=PaySlip2');", true);  //Open New Tab for Sever side code
            }
            catch { }   
        }

    }
}