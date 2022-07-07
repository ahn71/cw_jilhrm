using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class employee_status_info : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            loadEmployeeInfoStatus();
        }
        private void loadEmployeeInfoStatus()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select distinct EmpCardNo,EmpName,EmpType,Email,MobileNo,NationIDCardNo,EmpStatusName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,Remarks from v_Personnel_EmpCurrentStatus "
                    +" UNION "
                    +" select  distinct EmpCardNo,EmpName,EmpType,Email,MobileNo,NationIDCardNo,EmpStatusName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate ,Remarks  from v_Personnel_EmpSeparation_Log  ", dt = new DataTable(), sqlDB.connection);
                gvSeperationListLog.DataSource = dt;
                gvSeperationListLog.DataBind();
            }
            catch { }
        }

        protected void gvSeperationListLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                    if (e.Row.Cells[7].Text == "Regular")
                    e.Row.BackColor=Color.SkyBlue;
                    else if (e.Row.Cells[7].Text == "Resigned")
                        e.Row.BackColor = Color.MediumVioletRed;
                    else if (e.Row.Cells[7].Text == "Terminate")
                        e.Row.BackColor = Color.Yellow;
                }
            }
            catch { }
        }
    }
}