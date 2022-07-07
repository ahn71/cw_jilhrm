using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.others
{
    public partial class user_profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {

                setPrivilege();
            }
        }

        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                loadUserInfo(getUserId);
            }
            catch { }
        }

        void loadUserInfo(string UserId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select EmpName,EmpCardNo,SftName,DptName,DsgName,CompanyName,Address,Format(EmpJoiningDate,'dd-MMMM-yyyy') as EmpJoiningDate from v_Personnel_EmpCurrentStatus where EmpId =(select EmpId from UserAccount where UserId=" + UserId + ")", dt);

             

                lblName.InnerText = dt.Rows[0]["EmpName"].ToString();
                lblCardNo.InnerText = dt.Rows[0]["EmpCardNo"].ToString();
                lblUserType.InnerText = ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString());
                lblShift.InnerText = dt.Rows[0]["SftName"].ToString();
                lblDepartment.InnerText = dt.Rows[0]["DptName"].ToString();
                lblDesignation.InnerText = dt.Rows[0]["DsgName"].ToString();
                lblJoiningDate.InnerText = dt.Rows[0]["EmpJoiningDate"].ToString();

                lblCompanyTitle.InnerText = dt.Rows[0]["CompanyName"].ToString();
                lblAddress.InnerText = dt.Rows[0]["Address"].ToString();
                
            }
            catch { }
        }
    }
}