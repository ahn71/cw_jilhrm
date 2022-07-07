using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            if (!IsPostBack)
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                if (getCookies == null || getCookies.Value == "")
                {
                    Response.Redirect("~/ControlPanel/Login.aspx");

                }
                string getUserId = getCookies["__getUserId__"].ToString();
                string getUserType = getCookies["__getUserType__"].ToString();
                checkUserPrivilege.PrivilegeByModule(getUserType, getUserId,mSettings,mPersonnel,mLeave,mAttendance,mPayroll,mTools);
            }
            }
            catch { Response.Redirect("~/ControlPanel/Login.aspx"); }
        }

       

       
    }
}