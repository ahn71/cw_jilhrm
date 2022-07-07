using adviitRuntimeScripting;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class Reports1 : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {

            CrystalReportViewer1.Visible = true;

            ReportDocument rDoc = new ReportDocument();

            sqlDB.fillDataTable("Select * From v_AppoinmentLetter where SN=3110", dt = new DataTable());

            //DataTable dtable = new DataTable(); // data table name

            //dtable.TableName = "Crystal Report ";  // Crystal Report Name
            string path = Server.MapPath("/All Report/AppointmentLetter.rpt");
            rDoc.Load(Server.MapPath("/All Report/AppointmentLetter.rpt")); // Your .rpt file path

            rDoc.SetDataSource(dt); //set dataset to the report viewer.
            rDoc.SetParameterValue("CompanyName", "সিগমা ফ্যাশন্স লিমিটেড");
            rDoc.SetParameterValue("CompanyAddress", "আর এস-৬১ ধনাইড, তাজপুর রোড,আশুলিয়া, সাভার, ঢাকা ");
            rDoc.SetParameterValue("Age", "25");


            CrystalReportViewer1.ReportSource = rDoc;
            CrystalReportViewer1.DataBind();
            this.Focus();
        }
    }
}