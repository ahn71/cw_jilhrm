using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using BG.Common;

using System.Data;

namespace SigmaERP.attendance
{
    public partial class daily_atten_multiple_time : System.Web.UI.Page
    {
        string strSQL = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            if (!IsPostBack)
            {
                LoadDropDown();
            }
        }
        private void LoadDropDown()
        {
            //DataTable DTLocal = new DataTable();
            string strSQLCompany = "select MonthID, MonthName from dbo.tblMonthSetup";
           
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

            string strUrl = "ReportViewer.aspx?RepName=DailyAttenMultipleTime&MonthId='" + ddlMonthID.SelectedValue + "'&MonthName='" + ddlMonthID.SelectedItem.Text + "'&CardNO=" + txtCardNo.Text.Trim() + "";
                Response.Redirect(strUrl);
            
        }
    }
}