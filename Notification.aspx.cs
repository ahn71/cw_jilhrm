using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class Notification : System.Web.UI.Page
    {
        DataTable dt;
        string sql = "";
        string cmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                try { string[] query = Request.QueryString["for"].ToString().Split('-');
                if (query[0] == "ln")
                    loadLateNotification();
                else if (query[0] == "pn")
                    loadPermanentNotification();
                else if (query[0] == "bn")
                    loadBirthdayNotification();
                }
                catch { }
            }
        }
        private void loadLateNotification()
        {
            if (Session["__GetUserType__"].ToString().Equals("User"))
            {
                sql = "SELECT ln.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,ln.LateTime,convert(varchar(10), ln.Date,105) as Date FROM nf_LateNotification ln inner join v_EmployeeDetails ed on ln.EmpID=ed.EmpId   where ln.EmpId='" + Session["__GetEmpId__"].ToString() + "'   order by Date desc";
                cmd = "update nf_LateNotification set EmpSeen=1 where EmpID='" + Session["__GetEmpId__"].ToString() + "' and EmpSeen=0";
            }
            else if (Session["__GetUserType__"].ToString().Equals("Admin") || Session["__GetEmpId__"].ToString().Equals("00000001"))
            {
                sql = "SELECT ln.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,ln.LateTime,convert(varchar(10), ln.Date,105) as Date FROM nf_LateNotification ln inner join v_EmployeeDetails ed on ln.EmpID=ed.EmpId   where ln.AdminID='" + Session["__GetEmpId__"].ToString() + "' order by Date desc";
                cmd = "update nf_LateNotification set AdminSeen=1 where AdminId='" + Session["__GetEmpId__"].ToString() + "' and AdminSeen=0";
            }
            sqlDB.fillDataTable(sql, dt = new DataTable());
            if (dt==null || dt.Rows.Count == 0)
            {
               
                gvLateNotification.DataSource = null;
                gvLateNotification.DataBind();
                return;
            }
            gvLateNotification.DataSource = dt;
            gvLateNotification.DataBind();
            seenLateNotification(cmd);
        }
        private void loadBirthdayNotification()
        {
            if (Session["__GetUserType__"].ToString().Equals("User"))
            {
                sql = "SELECT ln.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,convert(varchar(10), ln.BirthDay,105) as BirthDay FROM nf_BirthdayNotification ln inner join v_EmployeeDetails ed on ln.EmpID=ed.EmpId   where ln.EmpId='" + Session["__GetEmpId__"].ToString() + "'   order by BirthDay desc";
                cmd = "update nf_BirthdayNotification set EmpSeen=1 where EmpID='" + Session["__GetEmpId__"].ToString() + "' and EmpSeen=0";
            }
            else if (Session["__GetUserType__"].ToString().Equals("Admin") || Session["__GetEmpId__"].ToString().Equals("00000001"))
            {
                sql = "SELECT ln.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,convert(varchar(10), ln.BirthDay,105) as BirthDay FROM nf_BirthdayNotification ln inner join v_EmployeeDetails ed on ln.EmpID=ed.EmpId   where ln.AdminID='" + Session["__GetEmpId__"].ToString() + "' order by BirthDay desc";
                cmd = "update nf_BirthdayNotification set AdminSeen=1 where AdminId='" + Session["__GetEmpId__"].ToString() + "' and AdminSeen=0";
            }
            sqlDB.fillDataTable(sql, dt = new DataTable());
            if (dt == null || dt.Rows.Count == 0)
            {

                gvBirthDayNotification.DataSource = null;
                gvBirthDayNotification.DataBind();
                return;
            }
            gvBirthDayNotification.DataSource = dt;
            gvBirthDayNotification.DataBind();
            seenLateNotification(cmd);
        }
        private void seenLateNotification(string sql) 
        {
            SqlCommand cmd = new SqlCommand(sql, sqlDB.connection);
            cmd.ExecuteNonQuery();
        }
        private void loadPermanentNotification()
        {

            //if (Session["__GetUserType__"].ToString().Equals("User"))
            //{
            //    sql = "SELECT pn.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,convert(varchar(10), pn.ActivedDate,105) as Date FROM nf_PermanentNotification pn inner join v_EmployeeDetails ed on pn.EmpID=ed.EmpId   where pn.EmpId='" + Session["__GetEmpId__"].ToString() + "'  order by ActivedDate desc";
            //    cmd = "update nf_PermanentNotification set EmpSeen=1 where EmpID='" + Session["__GetEmpId__"].ToString() + "' and EmpSeen=0";
            //}
            //else if (Session["__GetUserType__"].ToString().Equals("Admin") || Session["__GetEmpId__"].ToString().Equals("00000001"))
            //{
            //    sql = "SELECT pn.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,convert(varchar(10), pn.ActivedDate,105) as Date FROM nf_PermanentNotification pn inner join v_EmployeeDetails ed on pn.EmpID=ed.EmpId   where pn.AdminId='" + Session["__GetEmpId__"].ToString() + "'   order by ActivedDate desc";
            //    cmd = "update nf_PermanentNotification set AdminSeen=1 where AdminId='" + Session["__GetEmpId__"].ToString() + "' and AdminSeen=0";
            //}
            if (Session["__GetEmpId__"].ToString().Equals("00000001"))
            {
                sql = "SELECT pn.EmpID,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,'may be this employee permanent on '+ convert(varchar(10), pn.ActivedDate,105) as Date FROM nf_PermanentNotification pn inner join v_EmployeeDetails ed on pn.EmpID=ed.EmpId   where pn.AdminId='" + Session["__GetEmpId__"].ToString() + "'   order by ActivedDate desc";
                cmd = "update nf_PermanentNotification set AdminSeen=1 where AdminId='" + Session["__GetEmpId__"].ToString() + "' and AdminSeen=0";
            }
            sqlDB.fillDataTable(sql, dt = new DataTable());
            if (dt == null || dt.Rows.Count == 0)
            {

                gvPermanentNotification.DataSource = null;
                gvPermanentNotification.DataBind();
                return;
            }
            gvPermanentNotification.DataSource = dt;
            gvPermanentNotification.DataBind();
            seenLateNotification(cmd);
        }

        
    }
}