using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class MaternityLetterofAuthority : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
             //   classes.commonTask.loadMaternityEmpCardNo(ddlEmpCardNo);
            }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='MaternityLetterofAuthority.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnpreview.CssClass = "";

                            btnpreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
           //     sqlDB.fillDataTable("Select EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,NumberofChild From v_Personnel_EmpCurrentStatus where SN=" + ddlEmpCardNo.SelectedValue + "", dt);
                if (dt.Rows.Count > 0)
                {
                    Session["__LetterofAuthority__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LetterofAuthority');", true);  //Open New Tab for Sever side code
                }
                else
                    lblMessage.InnerText = "warning->No Data Available";
            }
            catch { }
        }
    }
}