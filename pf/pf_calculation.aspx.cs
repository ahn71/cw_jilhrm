using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.pf
{
    public partial class pf_calculation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();

            }
            lblMessage.InnerText = "";
        }
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__preRIndex__"] = "No";
                string getUserId = getCookies["__getUserId__"].ToString();

                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "pf_calculation.aspx", ddlCompanyName, btnpfcalculation);
                                                           
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];                
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.Employee.LoadEmpCardNoWithNameByCompanyForPf(ddlEmpCardNo, ddlCompanyName.SelectedValue);
            }


            catch { }

        }

        protected void btnpfcalculation_Click(object sender, EventArgs e)
        {
            try
            {

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                if (txtFrommonth.Text.Trim() == "") 
                {
                    lblMessage.InnerText = "warning-> Please select pf from month !";
                    txtFrommonth.Focus();
                    return;
                }
                if (txttomonth.Text.Trim() == "")
                {
                    lblMessage.InnerText = "warning-> Please select pf to month !";
                    txttomonth.Focus();
                    return;
                }
                DateTime fmonth = Convert.ToDateTime(txtFrommonth.Text.ToString());
                DateTime tomonth = Convert.ToDateTime(txttomonth.Text.ToString());
                string setPredicate = "";
                DateTime fm = new DateTime();
                for (fm = fmonth; fm <= tomonth; fm = fm.AddMonths(1))
                {
                    if(setPredicate=="")
                    {
                        setPredicate="'"+fm.ToString("MMM-yyyy")+"'";
                    }
                    else
                    {
                        setPredicate = setPredicate + ",'" + fm.ToString("MMM-yyyy")+"'";
                    }
                    
                }
                string emp= (ddlEmpCardNo.SelectedValue.Equals("0"))?"":" and EmpId='"+ddlEmpCardNo.SelectedValue+"'"; 
                setPredicate = "in(" + setPredicate + ")";

              
                    DataTable dt = new DataTable();


                    sqlDB.fillDataTable("Select EmpId,PfEmpContribution,PFAmount,RateofInterest,PfOpeningBalance from v_Personnel_EmpCurrentStatus "
                    +"where PfMember='1' and EmpStatus in('1','8') and IsActive='1'  and CompanyId='" + ddlCompanyName.SelectedValue + "' "+emp+" ", dt);
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        SqlCommand cmd = new SqlCommand("Delete From PF_CalculationDetails where EmpId='" + dt.Rows[i]["EmpId"].ToString() + "' and MonthName "+setPredicate+"", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                        float openingbalance = float.Parse(dt.Rows[i]["PfOpeningBalance"].ToString());
                        DataTable dtBalance = new DataTable();
                        sqlDB.fillDataTable("Select Isnull(SUM(EmpContributionAmount+EmprContributionAmount),0) as Balance From PF_CalculationDetails where EmpId='" + dt.Rows[i]["EmpId"].ToString() + "'", dtBalance);
                        float totalbalance = openingbalance + float.Parse(dtBalance.Rows[0]["Balance"].ToString());
                        fm=new DateTime();
                        for (fm = fmonth; fm <= tomonth; fm = fm.AddMonths(1))
                        {
                            float interesamount = (totalbalance/100)/12;
                            string[] getColumns = { "MonthName", "EmpId", "OpeningBalance", "EmpContributionPer", "EmpContributionAmount", "EmprContributionPer", "EmprContributionAmount"};
                            string[] getValues = { fm.ToString("MMM-yyyy"), dt.Rows[i]["EmpId"].ToString(),totalbalance.ToString(), dt.Rows[i]["PfEmpContribution"].ToString(), dt.Rows[i]["PFAmount"].ToString(), dt.Rows[i]["PfEmpContribution"].ToString(), dt.Rows[i]["PFAmount"].ToString()};

                            SQLOperation.forSaveValue("PF_CalculationDetails", getColumns, getValues, sqlDB.connection);
                           

                            totalbalance =totalbalance+(float.Parse(dt.Rows[i]["PFAmount"].ToString())*2);
                        }

                    }
                    lblMessage.InnerText = "success->PF Calculation Successfully"; 

            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoWithNameByCompanyForPf(ddlEmpCardNo, ddlCompanyName.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }
    }
}