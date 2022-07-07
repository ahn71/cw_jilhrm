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
    public partial class AdvanceDeductionMonthList : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadExistsAdvance();
            }
        }

        private void loadExistsAdvance()
        {
            try
            {
                dt = new DataTable();

                sqlDB.fillDataTable("select SL,AdvanceId,EmpName,EmpCardNo,AdvanceAmount,InstallmentNo,InstallmentAmount,Format(StartMonth,'MM-yyyy') as StartMonth,convert(varchar(11),EntryDate,105) as EntryDate ,EmpType,PaidInstallmentNo from v_Payroll_AdvanceInfo order by SL desc", dt = new DataTable());
                if (dt.Rows.Count > 0) gvAdvaceList.DataSource = dt;
                else gvAdvaceList.DataSource = null;
                gvAdvaceList.DataBind();
            }
            catch { }
        }

        protected void gvAdvaceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int rIndex=Convert.ToInt32(e.CommandArgument.ToString());
                sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_AdvanceSetting where AdvanceId ='" + gvAdvaceList.DataKeys[rIndex].Value.ToString() + "'",dt=new DataTable ());
                gvSettingsMonths.DataSource = dt;
                gvSettingsMonths.DataBind();
                mpe1.Show();
            }
            catch { }
        }
    }
}