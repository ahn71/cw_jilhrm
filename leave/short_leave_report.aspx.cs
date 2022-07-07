using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class short_leave_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            //string aa = ddlDepartment.SelectedValue;
            string strUrl = "LeaveReportViewer.aspx?RepName=ShortLeave&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "";
            Response.Redirect(strUrl);
        }
    }
}