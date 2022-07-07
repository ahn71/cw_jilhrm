using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class holyday_list_all1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadDropDown();
                LoagGrid();
            }
        }

        void LoagGrid()
        {
//            string strSQL = @"select [MonthId],div.DName, d.DptName, [DateIn],
//                            CONVERT(varchar(2),TimeInHr)+':'+CONVERT(varchar(4),TimeInMin) as TimeIn,
//                            CONVERT(varchar(2),TimeOutForLunchHr)+':'+CONVERT(varchar(4),TimeOutForLunchMin) as TimeOutForLunch ,
//                            CONVERT(varchar(2),TimeInAfterLunchHr)+':'+CONVERT(varchar(4),TimeInAfterLunchMin) as TimeInAfterLunch,
//                            CONVERT(varchar(2),TimeOutHr)+':'+CONVERT(varchar(4),TimeOutMin) as TimeOut
//                              from [dbo].[tblDepartmentWiseAttendance] dw 
//                              Join [dbo].[HRD_Department] d on d.DptCode=dw.DepartmentId
//                              join [dbo].[HRD_Division] div on div.DId=dw.DivisionId";
            //DataTable DTLocal = new DataTable();
            //ConManager.DBConnection("Sigma");
            //ConManager.OpenDataTableThroughAdapter(strSQL, out DTLocal, true);

            //gvDepartmentWiseAttencance.DataSource = DTLocal;
            //gvDepartmentWiseAttencance.DataBind();
        }
    }
}