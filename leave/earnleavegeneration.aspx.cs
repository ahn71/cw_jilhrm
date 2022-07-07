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

namespace SigmaERP.leave
{
    public partial class earnleavegeneration : System.Web.UI.Page
    {
        string sqlCmd = "";
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                txtStartDate.Text =  DateTime.Now.Year.ToString();
               
                setPrivilege();
                loadList();               
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
            
        }
        private void setPrivilege()
        {
            try
            {


                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "LeaveConfig.aspx", ddlCompanyList, gvEarnLeaveGenerationList, btnGenerate);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
              


            }
            catch { }

        }
        private void loadList()
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            earnleaveGeneration();
        }
        private void earnleaveGeneration()
        {
            try
            {
                if (txtStartDate.Text.Trim().Length==0)
                {
                    //lblMessage.InnerText = "warning-> Please Check Selected Year.";
                    //imgLoading.Visible = false;
                    txtStartDate.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "msg('warning','Please Select The Year');", true);
                    return;
                }
                DateTime startDate =DateTime.Parse(txtStartDate.Text.Trim()+"-01-01");
                DateTime endDate = DateTime.Parse(txtStartDate.Text.Trim() + "-12-31");
                if (endDate > DateTime.Now)
                {
                    //lblMessage.InnerText = "warning-> Please Check Selected Year.";
                    //imgLoading.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "msg('warning','Please Check Selected Year');", true);
                    return;
                }
                string EmpIDforIndividual = "";
                if (txtEmpCardNo.Text.Trim() != "")
                {
                    sqlCmd = "select EmpId from Personnel_EmployeeInfo where EmpCardNo like '%" + txtEmpCardNo.Text.Trim() + "'";
                    sqlDB.fillDataTable(sqlCmd, dt = new DataTable());// check valid employee 
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        EmpIDforIndividual = dt.Rows[0]["EmpId"].ToString();
                    }
                    else
                    {
                        lblMessage.InnerText = "warning-> Invalid card no!";
                        imgLoading.Visible = false;
                        return;
                    }
                }

                string year = endDate.AddDays(1).ToString("yyyy-MM-dd");
                deleteExGeneratedData(EmpIDforIndividual, year);
                if (EmpIDforIndividual != "")
                    EmpIDforIndividual = " and ela.EmpId='" + EmpIDforIndividual + "'";
                sqlCmd = "select ela.EmpId from EarnLeave_Activationlog ela inner join Personnel_EmpCurrentStatus pcs on ela.EmpID=pcs.EmpId and pcs.IsActive=1 where ActiveFrom <='"+ endDate.ToString("yyyy-MM-dd")+ "' and ela.IsActive=1 and pcs.EmpStatus in(1,8) "+ EmpIDforIndividual;
                DataTable dtEmp = new DataTable();
                dtEmp = CRUD.ExecuteReturnDataTable(sqlCmd,sqlDB.connection);
                if (dtEmp != null && dtEmp.Rows.Count > 0)
                {
                    int maxForwardNumber = 10;
                    int currentEarnLeaveDays = getCurrentEarnLeaveDays(ddlCompanyList.SelectedValue);
                    for (int i=0;i< dtEmp.Rows.Count;i++)
                    {
                        int reservedDaysForNext = 0;
                        string empID = dtEmp.Rows[i]["EmpID"].ToString();
                        int preEarnLeaveDays = getPreEarnLeaveDays(empID, startDate,endDate);
                        int enjoyedEarnLeaveDays = getEnjoyedEarnLeaveDays(empID, startDate,endDate);

                        reservedDaysForNext = currentEarnLeaveDays - enjoyedEarnLeaveDays;
                        if (reservedDaysForNext > maxForwardNumber)
                            reservedDaysForNext = maxForwardNumber;
                        reservedDaysForNext += preEarnLeaveDays;

                        saveToCarryforward(empID, reservedDaysForNext,year);
                    }
                   
                }
                //lblMessage.InnerText = "success-> Successfully Generated.";
                //imgLoading.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "msg('success','Successfully Generated');", true);

            }
            catch (Exception ex)
            {
                //lblMessage.InnerText = "error-> Unable to Generate.("+ex.Message+")";
                //imgLoading.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "msg('error','Unable to Generate.(" + ex.Message + ")');", true);
            }
        }
        private int getPreEarnLeaveDays(string empID, DateTime startDate, DateTime endDate)
        {
            try {
                sqlCmd = "select ELDays from Leave_EarnLeaveCarriedForward  where Year >= '"+ startDate.ToString("yyyy-MM-dd")+ "' AND Year<= '"+endDate.ToString("yyyy-MM-dd")+"'  and EmpId='"+ empID + "'";
                dt = new DataTable();
                dt = CRUD.ExecuteReturnDataTable(sqlCmd, sqlDB.connection);
                if (dt != null && dt.Rows.Count > 0)
                  return int.Parse(dt.Rows[0]["ELDays"].ToString());
                
                return 0;
            } catch(Exception ex) { return 0; }
        }
        private int getEnjoyedEarnLeaveDays(string empID, DateTime startDate, DateTime endDate)
        {
            try
            {
                sqlCmd = "select sum(TotalDays) as ELDays  from v_Leave_LeaveApplication where ShortName='a/l' and IsApproved=1 and FromDate >= '" + startDate.ToString("yyyy-MM-dd") + "' AND FromDate <= '" + endDate.ToString("yyyy-MM-dd") + "'  and EmpId='" + empID + "'";

                dt = new DataTable();
                dt = CRUD.ExecuteReturnDataTable(sqlCmd, sqlDB.connection);
                if (dt != null && dt.Rows.Count > 0)
                    return int.Parse(dt.Rows[0]["ELDays"].ToString());

                return 0;
            }
            catch (Exception ex) { return 0; }
        }
        private int getCurrentEarnLeaveDays(string companyId)
        {
            try
            {
                sqlCmd = "select LeaveDays as ELDays from tblLeaveConfig where ShortName='a/l' and CompanyId='" + companyId + "'";

                dt = new DataTable();
                dt = CRUD.ExecuteReturnDataTable(sqlCmd, sqlDB.connection);
                if (dt != null && dt.Rows.Count > 0)
                    return int.Parse(dt.Rows[0]["ELDays"].ToString());

                return 0;
            }
            catch (Exception ex) { return 0; }
        }
        private bool saveToCarryforward(string empID,int elDays,string year)
        {
            try
            {
                sqlCmd = "INSERT INTO [dbo].[Leave_EarnLeaveCarriedForward] ([EmpID],[ELDays],[Year]) VALUES('"+ empID + "',"+elDays+",'"+ year + "')";
                return CRUD.Execute(sqlCmd, sqlDB.connection);               
            }
            catch (Exception ex) { return false; }
        }

        private bool deleteExGeneratedData(string empID,string year)
        {
            try
            {
                if (empID == "")
                    sqlCmd = "delete Leave_EarnLeaveCarriedForward where year='"+ year + "'";
                else
                    sqlCmd = "delete Leave_EarnLeaveCarriedForward where year='"+ year + "' and EmpId='"+ empID + "'";
                return CRUD.Execute(sqlCmd, sqlDB.connection);
            }
            catch (Exception ex) { lblMessage.InnerText = "error-> " + ex.Message; return false; }
        }

    }
}