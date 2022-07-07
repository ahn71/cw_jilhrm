using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class chatnotify : System.Web.UI.Page
    {


        public static string sqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string getStatus = Request.QueryString["f"];
            if (!string.IsNullOrEmpty(getStatus))
            {
                signout(); // so=singout
                return;
            }
        }

        [WebMethod]
        public static string loadMessage(string ReceiverId)
        {
            try
            {

                string getUserId = HttpContext.Current.Session["__GetUID__"].ToString();
                string getCompanyId = HttpContext.Current.Session["__GetCompanyId__"].ToString();
                string geLvOnlyDpt = HttpContext.Current.Session["__GetLvOnlyDpt__"].ToString();
                string getDptId = HttpContext.Current.Session["__GetDptId__"].ToString();
              
              

                DataTable dt;
                string notify = "";
                // for load leave
                //if (geLvOnlyDpt != "True")
                //{

                //    sqlDB.fillDataTable("SELECT Leave_LeaveApplication.EmpId FROM UserAccount inner join Leave_LeaveApplication on UserAccount.LvAuthorityOrder=Leave_LeaveApplication.LeaveProcessingOrder where UserId='" + getUserId + "' and Leave_LeaveApplication.IsApproved='False'", dt = new DataTable());

                //    if (dt.Rows.Count > 0)
                //    {
                //        notify = "forlv_" + dt.Rows.Count;
                //    }
                //    dt = new DataTable();
                //    // for load short leave
                //    sqlDB.fillDataTable("SELECT Leave_ShortLeave.EmpId FROM UserAccount inner join Leave_ShortLeave on UserAccount.LvAuthorityOrder=Leave_ShortLeave.LeaveProcessingOrder where UserId='" + getUserId + "' and Leave_ShortLeave.LvStatus='0'", dt = new DataTable());
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (notify != "")
                //            notify += "_" + dt.Rows.Count;
                //        else
                //        {
                //            notify = "forshortlv_" + dt.Rows.Count;
                //        }
                //    }
                //}
                //else 
                //{

                //    sqlDB.fillDataTable("SELECT Leave_LeaveApplication.EmpId FROM UserAccount inner join Leave_LeaveApplication on UserAccount.LvAuthorityOrder=Leave_LeaveApplication.LeaveProcessingOrder where UserId='" + getUserId + "' and Leave_LeaveApplication.DptId='"+getDptId+"' and Leave_LeaveApplication.IsApproved='False'", dt = new DataTable());

                //    if (dt.Rows.Count > 0)
                //    {
                //        notify = "forlv_" + dt.Rows.Count;
                //    }
                //    dt = new DataTable();
                //    // for load short leave
                //    sqlDB.fillDataTable("SELECT Leave_ShortLeave.EmpId FROM UserAccount inner join Leave_ShortLeave on UserAccount.LvAuthorityOrder=Leave_ShortLeave.LeaveProcessingOrder where UserId='" + getUserId + "' and Leave_ShortLeave.DptId='" + getDptId + "' and Leave_ShortLeave.LvStatus='0'", dt = new DataTable());
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (notify != "")
                //            notify += "_" + dt.Rows.Count;
                //        else
                //        {
                //            notify = "forshortlv_" + dt.Rows.Count;
                //        }
                //    }
                //}

                // for load leave
                //sqlCmd = "SELECT Leave_LeaveApplication.EmpId FROM UserAccount inner join Leave_LeaveApplication on UserAccount.LvAuthorityOrder=Leave_LeaveApplication.LeaveProcessingOrder where UserId='" + getUserId + "' and Leave_LeaveApplication.DptId='" + getDptId + "' and Leave_LeaveApplication.IsApproved='False'";
                sqlCmd = "SELECT lv.EmpId FROM tblLeaveAuthorityAccessControl ac inner join Leave_LeaveApplication lv on ac.AuthorityPosition=lv.LeaveProcessingOrder and ac.EmpID=lv.EmpId and  ac.AuthorityID='" + getUserId + "'  and lv.IsApproved='False'";
                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    notify = "forlv_" + dt.Rows.Count;
                }

                // for load short leave              
                sqlCmd = "SELECT Leave_ShortLeave.EmpId FROM UserAccount inner join Leave_ShortLeave on UserAccount.LvAuthorityOrder=Leave_ShortLeave.LeaveProcessingOrder where UserId='" + getUserId + "' and Leave_ShortLeave.DptId='" + getDptId + "' and Leave_ShortLeave.LvStatus='0'";
                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    if (notify != "")
                        notify += "_" + dt.Rows.Count;
                    else
                    {
                        notify = "forshortlv_" + dt.Rows.Count;
                    }
                }
                return notify;
               
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [WebMethod]
        public static string loadOutDuty(string ReceiverId)
        {
            try
            {

                string getUserId = HttpContext.Current.Session["__GetUID__"].ToString();
                string getCompanyId = HttpContext.Current.Session["__GetCompanyId__"].ToString();
                string geODOnlyDpt = HttpContext.Current.Session["__GetODOnlyDpt__"].ToString();
                string getDptId = HttpContext.Current.Session["__GetDptId__"].ToString();



                DataTable dt;
                string notify = "";

                //if (geODOnlyDpt != "True")
                //{
                //    sqlDB.fillDataTable("SELECT tblOutDuty.EmpId FROM UserAccount inner join tblOutDuty on UserAccount.ODAuthorityOrder=tblOutDuty.Processing where UserId='" + getUserId + "' and tblOutDuty.Status='0'", dt = new DataTable());
                //    if (dt.Rows.Count > 0)
                //    {
                //        notify = dt.Rows.Count.ToString();
                //    }
                //}
                //else
                //{
                //    sqlDB.fillDataTable("SELECT v_tblOutDuty.EmpId FROM UserAccount inner join v_tblOutDuty on UserAccount.ODAuthorityOrder=v_tblOutDuty.Processing where  IsActive=1 and UserId='" + getUserId + "' and v_tblOutDuty.DptId='"+ getDptId + "' and v_tblOutDuty.Status='0'", dt = new DataTable());
                //    if (dt.Rows.Count > 0)
                //    {
                //        notify = dt.Rows.Count.ToString();
                //    }
                //}
                sqlCmd = "SELECT od.EmpId FROM v_tblOutDuty od inner join tblOutDutyAuthorityAccessControl ac on ac.AuthorityPosition=od.Processing and ac.EmpID=od.EmpId and ac.AuthorityID='"+getUserId+"' and od.Status='0' and  IsActive=1 ";
                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    notify = dt.Rows.Count.ToString();
                }
                return notify;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [WebMethod]
        public static string loadLateNotification(string ReceiverId)
        {
            try
            {
               
                string getEmpId = HttpContext.Current.Session["__GetEmpId__"].ToString();
                string getUserType = HttpContext.Current.Session["__GetUserType__"].ToString();
               
               

                // for late Notification
                //if (getUserType == "User" || getUserType == "Admin" || getEmpId == "00000001")
                //{
                    DataTable dt;
                    string notify = "";
                    if (getUserType == "User")
                        sqlDB.fillDataTable("SELECT * FROM nf_LateNotification  where EmpID='" + getEmpId + "' and EmpSeen=0  order by Date desc", dt = new DataTable());
                    else
                        sqlDB.fillDataTable("SELECT * FROM nf_LateNotification  where AdminID='" + getEmpId + "' and AdminSeen=0  order by Date desc", dt = new DataTable());
                    if (dt.Rows.Count > 0)
                    {
                        notify = dt.Rows.Count.ToString();
                    }

                    return notify;
                //}
                //else
                //    return "";

               

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [WebMethod]
        public static string loadBirthDayNotification(string ReceiverId)
        {
            try
            {

                string getEmpId = HttpContext.Current.Session["__GetEmpId__"].ToString();
                string getUserType = HttpContext.Current.Session["__GetUserType__"].ToString();



                // for late Notification
                //if (getUserType == "User" || getUserType == "Admin" || getEmpId == "00000001")
                //{
                    DataTable dt;
                    string notify = "";
                    if (getUserType == "User")
                        sqlDB.fillDataTable("SELECT BirthDay FROM nf_BirthdayNotification  where EmpID='" + getEmpId + "' and EmpSeen=0  order by BirthDay desc", dt = new DataTable());
                    else
                        sqlDB.fillDataTable("SELECT BirthDay FROM nf_BirthdayNotification  where AdminID='" + getEmpId + "' and AdminSeen=0  order by BirthDay desc", dt = new DataTable());
                    if (dt.Rows.Count > 0)
                    {
                        notify = dt.Rows.Count.ToString();
                    }

                    return notify;
                //}
                //else
                //    return "";



            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [WebMethod]
        public static string loadPermanentNotification(string ReceiverId)
        {
            try
            {

                string getEmpId = HttpContext.Current.Session["__GetEmpId__"].ToString();
                string getUserType = HttpContext.Current.Session["__GetUserType__"].ToString();
              //  if (getUserType == "User" || getUserType == "Admin" || getEmpId == "00000001")
                //if (getEmpId == "00000001")
                //{
                DataTable dt;
                string notify = "";

                // for late Notification
                //if (getUserType == "User")
                //    sqlDB.fillDataTable("SELECT * FROM nf_PermanentNotification  where EmpID='" + getEmpId + "' and EmpSeen=0  order by ActivedDate desc", dt = new DataTable());
                //else
                    sqlDB.fillDataTable("SELECT * FROM nf_PermanentNotification  where AdminID='" + getEmpId + "' and AdminSeen=0  order by ActivedDate desc", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    notify = dt.Rows.Count.ToString();
                }

                return notify;
                //}
                //else
                //    return "";

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string updateLoginDateTime(string ReceiverId)
        {
            try
            {
                string getUserId = HttpContext.Current.Session["__GetUID__"].ToString();

                string dateFormat = "dd-MM-yyyy hh:mm:ss";
                string datTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                DateTime LoginDateTime = DateTime.ParseExact(datTime, dateFormat, CultureInfo.InvariantCulture);
                SqlCommand cmd = new SqlCommand("Update UserAccount Set IsLogin='1',LoginDateTime='" + LoginDateTime + "' where UserId=" + getUserId + "", sqlDB.connection);
                cmd.ExecuteNonQuery();

                return " ";
            }
            catch { return " "; }
        }

        public void signout()
        {
            try
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/ControlPanel/Login.aspx",true);

            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }
    }
}