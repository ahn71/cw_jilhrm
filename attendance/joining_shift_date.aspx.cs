using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class joining_shift_date : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
               // string aa = ddlDepartment.SelectedValue;
                string strUrl = "ReportViewer.aspx?RepName=JoiningAndShiftDate";
                Response.Redirect(strUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}