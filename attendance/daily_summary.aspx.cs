using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class daily_summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            if (!IsPostBack)
            {

            }

        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            
                string strUrl = "ReportViewer.aspx?RepName=DailySummary&ATTStatus=A&ATTDate='" + dptDate.Text.Trim() + "'";
                Response.Redirect(strUrl);
          
        }
    }
}