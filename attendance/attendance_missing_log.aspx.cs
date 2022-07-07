using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class attendance_missing_log : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                classes.mCommon_Module_For_AttendanceProcessing.LoadAttendanceMissingLog(gvAttMissingLog);
            }
        }
    }
}