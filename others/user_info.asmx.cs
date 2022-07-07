using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

namespace SigmaERP.others
{
    /// <summary>
    /// Summary description for user_info
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class user_info : System.Web.Services.WebService
    {

        [WebMethod]
        public List <string> getUserInfo(string getFirstName)
        {
            List<string> userList = new List<string>();

            SqlCommand cmd = new SqlCommand("getUserInfo",sqlDB.connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FullName", getFirstName);
           
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read()) userList.Add(sdr["UserFullName"].ToString());
           
            return userList;
        }

        [WebMethod]
        public string Logout()
        {
            try
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/ControlPanel/Login.aspx");
                return "Logout";
            }
            catch { return null; }
        }
    }
}
